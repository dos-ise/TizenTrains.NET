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

            _game = DI.ServiceLocator.GetService<IGame>();
            _interactionManager = DI.ServiceLocator.GetService<IInteractionManager>();

            _bitmap = new SKBitmap(_width, _height);
            using (SKCanvas canvas = new SKCanvas(_bitmap))
            {
                _canvasWrapper = new SKCanvasWrapper(canvas);
            }

            _imageView = new ImageView
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent
            };
            _window.Add(_imageView);

            _game.InitializeAsync(200, 200).GetAwaiter().GetResult();
            _game.SetSize(_width, _height);

            _window.KeyEvent += OnKeyEvent;

            Timer renderTimer = new Timer(33);
            renderTimer.Tick += OnRenderTick;
            renderTimer.Start();
        }

        private bool OnRenderTick(object source, Timer.TickEventArgs e)
        {
            return true;
        }


        private void OnKeyEvent(object source, Window.KeyEventArgs e)
        {

        }
    }
}
