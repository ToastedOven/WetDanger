﻿using R2API.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoistureUpset.NetMessages
{
    public static class Register
    {
        public static void Init()
        {
            NetworkingAPI.RegisterMessageType<SyncAnimation>();
            NetworkingAPI.RegisterMessageType<SyncAudio>();
            NetworkingAPI.RegisterMessageType<SyncAudioWithJotaroSubtitles>();
        }
    }
}
