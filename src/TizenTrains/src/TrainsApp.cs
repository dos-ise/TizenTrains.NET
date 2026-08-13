using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using SkiaSharp;
using Trains.NET.Rendering;
using Trains.NET.Rendering.Skia;
using Timer = Tizen.NUI.Timer;

namespace TizenTrains
{
    public class TrainsApp : NUIApplication
    {
        private Window _window;
        private ImageView _imageView;
        private IGame _game;
        private IInteractionManager _interactionManager;
        private SKBitmap _bitmap;
        private SKCanvasWrapper _canvasWrapper;
        private int _width;
        private int _height;

        // Virtueller Cursor für D-Pad-Steuerung.
        private int _cursorX;
        private int _cursorY;
        private const int CursorStep = 20;

        public TrainsApp() : base("", WindowMode.Opaque) { }

        protected override void OnCreate()
        {
            base.OnCreate();
            OnInitialize();
        }

        private void OnInitialize()
        {
            _window = Window.Default;
            _window.SetOpaqueState(true);

            _width = _window.Size.Width;
            _height = _window.Size.Height;
            _cursorX = _width / 2;
            _cursorY = _height / 2;

            _game = DI.ServiceLocator.GetService<IGame>();
            _interactionManager = DI.ServiceLocator.GetService<IInteractionManager>();

            _bitmap = new SKBitmap(_width, _height);

            _imageView = new ImageView
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent
            };
            _window.Add(_imageView);

            _game.InitializeAsync(200, 200).GetAwaiter().GetResult();
            _game.SetSize(_width, _height);

            _window.KeyEvent += OnKeyEvent;

            Timer renderTimer = new Timer(33); // ~30 FPS
            renderTimer.Tick += OnRenderTick;
            renderTimer.Start();
        }

        private bool OnRenderTick(object source, Timer.TickEventArgs e)
        {
            using (SKCanvas canvas = new SKCanvas(_bitmap))
            {
                canvas.Clear(SKColors.Black);
                _canvasWrapper ??= new SKCanvasWrapper(canvas);
                _game?.Render(_canvasWrapper);
            }

            using PixelBuffer pixelBuffer = new PixelBuffer((uint)_width, (uint)_height, PixelFormat.RGBA8888);

            IntPtr destPtr = pixelBuffer.GetBuffer();
            unsafe
            {
                System.Buffer.MemoryCopy(
                    (void*)_bitmap.GetPixels(),
                    (void*)destPtr,
                    _width * _height * 4,
                    _width * _height * 4);
            }

            PixelData pixelData = PixelBuffer.Convert(pixelBuffer);
            ImageUrl imageUrl = pixelData.GenerateUrl();
            _imageView.ResourceUrl = imageUrl.ToString();

            return true;
        }

        private void OnKeyEvent(object source, Window.KeyEventArgs e)
        {
            if (e.Key.State != Key.StateType.Down)
            {
                return;
            }

            Tizen.Log.Info("TizenTrains", e.Key.KeyPressedName);

            switch (e.Key.KeyPressedName)
            {
                case "Up":
                    _cursorY -= CursorStep;
                    _interactionManager?.PointerMove(_cursorX, _cursorY);
                    break;
                case "Down":
                    _cursorY += CursorStep;
                    _interactionManager?.PointerMove(_cursorX, _cursorY);
                    break;
                case "Left":
                    _cursorX -= CursorStep;
                    _interactionManager?.PointerMove(_cursorX, _cursorY);
                    break;
                case "Right":
                    _cursorX += CursorStep;
                    _interactionManager?.PointerMove(_cursorX, _cursorY);
                    break;
                case "Return":
                    _interactionManager?.PointerClick(_cursorX, _cursorY);
                    break;
                case "XF86Back":
                    Exit();
                    break;
            }
        }
    }
}
