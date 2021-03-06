﻿using IrcDotNet;
using System;
using Vokram.Core;
using Vokram.Core.Extensions;

namespace Vokram.Plugins
{
    public class Uptime : PluginBase
    {
        private DateTime _startTime = DateTime.Now;

        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!uptime", Callback);
        }

        private void Callback(IrcMessageEventArgs message)
        {
            var uptime = DateTime.Now - _startTime;

            var reply = message.CreateReply($"I was started {_startTime.ToLocalTime()}, and uptime is {uptime.ToString()}");
            Bot.SendMessage(reply);
        }
    }
}
