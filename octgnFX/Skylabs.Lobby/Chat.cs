﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.chatstates;
using agsXMPP.protocol.x.muc;

namespace Skylabs.Lobby
{
    public class Chat
    {
        private long _lastRid = 0;
        private Client _client;
        public List<NewChatRoom> Rooms; 
        private XmppClientConnection _xmpp;
        public long NextRid
        {
            get
            {
                _lastRid = _lastRid + 1 == long.MaxValue ? 0 : _lastRid+1;
                return _lastRid;
            }
        }
        public Chat(Client c, XmppClientConnection xmpp)
        {
            _client = c;
            Rooms = new List<NewChatRoom>();
            _xmpp = xmpp;
            _xmpp.OnMessage += XmppOnOnMessage;
        }

        private void XmppOnOnMessage(object sender , Message msg)
        {
            switch (msg.Type)
            {
                case MessageType.normal:
                    break;
                case MessageType.error:
                    break;
                case MessageType.chat:
                    switch (msg.Chatstate)
                    {
                        case Chatstate.None:
                            if (!RoomExists(new NewUser(msg.From)))
                            {
                                var nc = GetRoom(new NewUser(msg.From));
                                nc.OnMessage(sender , msg);
                            }
                            break;
                        case Chatstate.active:

                            break;
                        case Chatstate.inactive:
                            break;
                        case Chatstate.composing:
                            break;
                        case Chatstate.gone:
                            break;
                        case Chatstate.paused:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case MessageType.groupchat:
                    break;
                case MessageType.headline:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public bool RoomExists(NewUser otherUser) { return Rooms.SingleOrDefault(x => x.Users.Contains(otherUser) && !x.IsGroupChat) != null; }
        public NewChatRoom GetRoom(NewUser otherUser)
        {
            var ret = Rooms.SingleOrDefault(x => x.Users.Contains(otherUser) && !x.IsGroupChat)
                      ?? new NewChatRoom(NextRid,_xmpp,new NewUser(_xmpp.MyJID),otherUser);
            return ret;
        }
        public long SendMessage(NewUser to, string message)
        {
            Message m = new Message(to.User,message);
            _client.Xmpp.Send(m);
            return 0;
        }
    }
}