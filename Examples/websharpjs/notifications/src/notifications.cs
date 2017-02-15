using System;
using System.Threading.Tasks;
using System.Collections.Generic;

// Reference WebSharpJs
using WebSharpJs;

namespace Notifications
{
    public class Notifications
    {
        public async Task<object> AddNotifications(dynamic input)
        {
            Func<object, Task<object>> consoleLog = await WebSharp.CreateJavaScriptFunction(@"
                                return function (data, callback) {
                                    console.log(data);
                                    callback(null, null);
                                }
                            ");

            Func<object, Task<object>> addEventListener = await WebSharp.CreateJavaScriptFunction(@"
                                return function (element, callback) {
                                    // Add an EventListener to the DOM element
                                    // We only need the evt.srcElement.Id
                                    document.getElementById(element.Id).addEventListener('click', function click (evt) { element.click(evt.srcElement.id);  });
                                    callback(null, null);
                                }
                            ");

            Func<object, Task<object>> eventFormatter = await WebSharp.CreateJavaScriptFunction(@"
                                return function (event, callback) {
                                    //console.log(event);
                                    // We only pass in the SrcElement to be interrogated.
                                    event.Handler(event.SrcElement, null);
                                    callback(null, null);
                                }
                            ");

            try
            {

                // Create our Notification options for each button
                var options = new NotificationOptions[] {
                    
                    new NotificationOptions() {
                            Title = "Basic Notification",
                            Body = "Short message part",
                            NotificationDirection = NotificationDirection.ltr,
                            OnClick = new EventHandler(
                                (Func<object, Task<object>>)(async (evt) =>
                                { 
                                    var not = await Notification.FromObject((dynamic)evt);
                                    consoleLog($"Notification : {await not.GetTitle()} clicked.");
                                    //consoleLog(evt);
                                    //var dic = (Dictionary<string, object>)evt;
                                    // foreach(KeyValuePair<string, object> kv in (dynamic)evt)
                                    //      consoleLog($"field {kv.Key} - {kv.Value}" );
                                    return null;
                                }),
                                eventFormatter // Custom formatter to marshal the needed information back to EventHandler Handler function.
                                
                            ),
                            OnShow = new EventHandler(
                                (Func<object, Task<object>>)(async (evt) =>
                                { 
                                    consoleLog("On Show");
                                    return null;
                                }))

                        },
                    
                    new NotificationOptions() {
                        Title = "Content-Image Notification",
                        Body = "Short message plus a custom content image",
                        Image = (string)input.dirname + "/icon.png",
                            OnClick = new EventHandler(
                                (Func<object, Task<object>>)(async (evt) =>
                                { 
                                    consoleLog($"message clicked");
                                    return null;
                                }),
                                eventFormatter
                            ),
                            OnShow = new EventHandler(
                                (Func<object, Task<object>>)(async (evt) =>
                                { 
                                    consoleLog("On Show");
                                    return null;
                                }))

                    }
                };

                var doNotify = (Func<object, Task<object>>)(async (srcElementId) =>
                                    {
                                        consoleLog($"C# callback: Clicked {srcElementId}");
                                        if ((string)srcElementId == "basic")
                                        {
                                            var not = await Notification.Create(options[0].Title, options[0]);
                                            consoleLog($"Notification Title {await not.GetTitle()}");
                                            
                                        }
                                        else
                                        {
                                            var not = await Notification.Create(options[1].Title, options[1]);
                                            consoleLog($"Notification Title {await not.GetTitle()}");
                                        }
                                        return null;
                                    });

                var basicListener = new
                {
                    Id = "basic",
                    click = doNotify,
                };

                var imageListener = new
                {
                    Id = "image",
                    click = doNotify,
                };

                addEventListener(basicListener);
                addEventListener(imageListener);

            }
            catch (Exception exc) { consoleLog($"Exception: {exc.Message}"); }

            return null;
        }
    }

    public class EventHandler
    {
        public Func<object, Task<object>> Handler {get;set;}
        public Func<object, Task<object>> Formatter {get;set;}

        public EventHandler(Func<object, Task<object>> handler, Func<object, Task<object>> formatter = null)
        {
            Handler = handler;
            Formatter = formatter;
        } 
    }

    public enum NotificationDirection 
    {
        auto,
        ltr,
        rtl
    };

    public class NotificationOptions
    {
        public string Title {get;set;}
        public NotificationDirection NotificationDirection {get;set;} = NotificationDirection.auto;
        public string Language {get;set;}
        public string Body {get;set;}
        public string Tag {get;set;}
        public string Image {get;set;}
        public string Icon {get;set;}
        public string Badge {get;set;}
        public string Sound {get;set;}
        public string TimeStamp {get;set;}
        public bool Renotify {get;set;}
        public bool Silent {get;set;}
        public bool RequireInteraction {get;set;}
        public object Data {get;set;}

        public EventHandler OnClick {get;set;}
        public EventHandler OnError {get;set;}
        public EventHandler OnShow {get;set;}

        public Dictionary<string, object> AsDictionary()
        {
            var dic = new Dictionary<string,object>();
            dic.Add("title", Title);
            dic.Add("dir", NotificationDirection);
            if (!string.IsNullOrEmpty(Language))
                dic.Add("lang", Language);
            if (!string.IsNullOrEmpty(Body))
                dic.Add("body", Body);
            if (!string.IsNullOrEmpty(Tag))
                dic.Add("tag", Tag);
            if (!string.IsNullOrEmpty(Image))
                dic.Add("image", Image);
            if (!string.IsNullOrEmpty(Icon))
                dic.Add("icon", Icon);
            if (!string.IsNullOrEmpty(Badge))
                dic.Add("badge", Badge);
            if (!string.IsNullOrEmpty(Sound))
                dic.Add("sound", Sound);
            if (!string.IsNullOrEmpty(TimeStamp))
                dic.Add("timestamp", TimeStamp);
            dic.Add("renotify", Renotify);
            dic.Add("silent", Silent);
            dic.Add("requireInteraction", RequireInteraction);
            if (Data != null)
                dic.Add("data", Data);
            if (OnClick != null)
                dic.Add("onclick", new { handler = OnClick.Handler, formatter = OnClick.Formatter } );
            if (OnError != null)
                dic.Add("onerror", new { handler = OnError.Handler, formatter = OnError.Formatter });
            if (OnShow != null)
                dic.Add("onshow", new { handler = OnShow.Handler, formatter = OnShow.Formatter });
//   [SameObject] readonly attribute FrozenArray<unsigned long> vibrate;
//   [SameObject] readonly attribute FrozenArray<NotificationAction> actions;                
            return dic;
        }



    }
    public class Notification
    {
        
        public dynamic _notificationProxy;

        public static async Task<Notification> Create(string title, NotificationOptions options)
        {
            var proxy = new Notification();
            await proxy.Initialize(title, options);
            return proxy;
        }

        public static async Task<Notification> FromObject(dynamic srcElement)
        {
            var proxy = new Notification();
            proxy._notificationProxy = srcElement;
            return proxy;
        }

        private Notification()
        {

        }

        private async Task Initialize(string title, NotificationOptions options)
        {
            Func<object, Task<object>> notification = await WebSharp.CreateJavaScriptFunction(@"
                                return function (options, callback) {
                                    let notification = new Notification(options.title, options);
                                    
                                    let proxy = {};
                                    proxy.get_title = function (data, cb) {
                                        cb(null, notification.title);
                                    };
                                    
                                    proxy.get_dir = function (data, cb) {
                                        cb(null, notification.dir);
                                    };
                                    proxy.get_lang = function (data, cb) {
                                        cb(null, notification.lang);
                                    };
                                    proxy.get_body = function (data, cb) {
                                        cb(null, notification.body);
                                    };
                                    proxy.get_tag = function (data, cb) {
                                        cb(null, notification.tag);
                                    };
                                    proxy.get_image = function (data, cb) {
                                        cb(null, notification.image);
                                    };
                                    proxy.get_icon = function (data, cb) {
                                        cb(null, notification.icon);
                                    };
                                    proxy.get_badge = function (data, cb) {
                                        cb(null, notification.badge);
                                    };
                                    proxy.get_sound = function (data, cb) {
                                        cb(null, notification.sound);
                                    };
                                    proxy.get_timestamp = function (data, cb) {
                                        cb(null, notification.timestamp);
                                    };
                                    proxy.get_renotify = function (data, cb) {
                                        cb(null, notification.renotify);
                                    };
                                    proxy.get_silent = function (data, cb) {
                                        cb(null, notification.silent);
                                    };
                                    proxy.get_requireInteraction = function (data, cb) {
                                        cb(null, notification.requireInteraction);
                                    };
                                    proxy.get_data = function (data, cb) {
                                        cb(null, notification.data);
                                    };

//   [SameObject] readonly attribute FrozenArray<unsigned long> vibrate;
//   [SameObject] readonly attribute FrozenArray<NotificationAction> actions;
                                    
                                    if (options.onclick) 
                                    {  
                                        if (options.onclick.formatter)
                                        {
                                            
                                            notification.onclick = function (evt) {
                                                 let event = {};
                                                 event.Event = {};
                                                 event.SrcElement = proxy;
                                                 event.Handler = options.onclick.handler;
                                                 // very simple serialization of event
                                                 for ( name in evt ) {
                                                    event.Event[name] = evt[ name ];
                                                 };
                                                 options.onclick.formatter(event, null);
                                             }
                                        }
                                        else
                                            notification.onclick = options.onclick.handler;
                                    }
                                    if (options.onshow) 
                                    {  
                                        if (options.onshow.formatter)
                                        {
                                            
                                            notification.onshow = function (evt) {
                                                 let event = {};
                                                 event.Event = {};
                                                 event.SrcElement = proxy;
                                                 event.Handler = options.onshow.handler;
                                                 // very simple serialization of event
                                                 for ( name in evt ) {
                                                    event.Event[name] = evt[ name ];
                                                 };
                                                 options.onshow.formatter(event, null);
                                             }
                                        }
                                        else
                                            notification.onshow = options.onshow.handler(null, null);
                                    }

                                    if (options.onerror) 
                                    {  
                                        if (options.onerror.formatter)
                                        {
                                            
                                            notification.onerror = function (evt) {
                                                 let event = {};
                                                 event.Event = {};
                                                 event.SrcElement = proxy;
                                                 event.Handler = options.onerror.handler;
                                                 // very simple serialization of event
                                                 for ( name in evt ) {
                                                    event.Event[name] = evt[ name ];
                                                 };
                                                 options.onerror.formatter(event, null);
                                             }
                                        }
                                        else
                                            notification.onerror = options.onerror.handler;
                                    }
                                    
                                    //console.log(notification);
                                    callback(null, proxy);
                                }
                            ");

            if (!string.IsNullOrEmpty(title))                            
                options.Title = title;

            _notificationProxy = await notification(options.AsDictionary());
        }

        public async Task<string> GetTitle()
        {
            return await _notificationProxy.get_title((object)null);
        }
        public async Task<NotificationDirection> GetNotificationDirection()
        {
            var dir = await _notificationProxy.get_dir((object)null);
            NotificationDirection dirValue;
            if (Enum.TryParse((string)dir, true, out dirValue))  
                return dirValue;
            else
                return NotificationDirection.auto;
        }

        public async Task<string> GetLanguage()
        {
            return await _notificationProxy.get_lang((object)null);
        }
        
        public async Task<string> GetBody()
        {
            return await _notificationProxy.get_body((object)null);
        }
        public async Task<string> GetTag()
        {
            return await _notificationProxy.get_tag((object)null);
        }
        public async Task<string> GetImage()
        {
            return await _notificationProxy.get_imgae((object)null);
        }
        public async Task<string> GetIcon()
        {
            return await _notificationProxy.get_icon((object)null);
        }
        public async Task<string> GetBadge()
        {
            return await _notificationProxy.get_badge((object)null);
        }
        public async Task<string> GetSound()
        {
            return await _notificationProxy.get_souncd((object)null);
        }
//   [SameObject] readonly attribute FrozenArray<unsigned long> vibrate;

        public async Task<string> GetTimestamp()
        {
            return await _notificationProxy.get_timestamp((object)null);
        }

        public async Task<bool> GetRenotify()
        {
            return await _notificationProxy.get_renotify((object)null);
        }
        public async Task<bool> GetSilent()
        {
            return await _notificationProxy.get_silent((object)null);
        }
        public async Task<bool> GetRequireInteraction()
        {
            return await _notificationProxy.get_requireInteraction((object)null);
        }
        public async Task<object> GetData()
        {
            return await _notificationProxy.get_data((object)null);
        }

//   [SameObject] readonly attribute FrozenArray<NotificationAction> actions;        
    }
}