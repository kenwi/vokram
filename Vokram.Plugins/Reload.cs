﻿using System;
using IrcDotNet;
using IrcDotNet.Collections;
using Vokram.Core.Extensions;

namespace Vokram.Plugins
{
    public class Reload : PluginBase
    {
        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!reload", Callback);
        }

        private void Callback(IrcMessageEventArgs message)
        {
            Bot.Plugins.Clear();
            Bot.Plugins = Core.Utils.Plugins.LoadAll();
            Bot.UnSubscribeAllMessages();

            Bot.Plugins.ForEach(plugin => plugin.Initialize(Bot));
            Bot.SendMessage(message.CreateReply("Reloaded plugins."));
        }
    }
}