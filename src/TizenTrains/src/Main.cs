/*
 * Copyright (c) 2017 Samsung Electronics Co., Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;

namespace TizenTrains
{
    class TizenTrainsDemo
    {
        [STAThread]
        static int Main(string[] args)
        {
            //Tizen.Log.Debug("NUI", "Main() called");
            TrainsApp trainsApp = new TrainsApp();

            float version = 0.1f;
            Tizen.Log.Info("TizenTrains", "Trains.NET NUI TV-port. Version: " + version);

            // Process command line arguments.
            if (args.Length > 0)
            {
                string argumentList = "";
                for (int i = 0; i < args.Length; ++i)
                {
                    argumentList += args[i] + " ";
                }

                Tizen.Log.Info("TizenTrains", "Argument list: " + argumentList);
            }

            trainsApp.Run(args);
            return 0;
        }
    }
}
