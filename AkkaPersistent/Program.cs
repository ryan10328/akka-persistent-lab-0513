using System;
using System.IO;
using System.Linq;
using System.Threading;
using Akka.Actor;
using Akka.Configuration;
using Akka.Persistence.MongoDb;

namespace AkkaPersistent
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = File.ReadAllText("config.hocon");
            var config = ConfigurationFactory.ParseString(str);

            using (var actorSystem = ActorSystem.Create("root-actor", config))
            {
                MongoDbPersistence.Get(actorSystem);
                var fooActor = actorSystem.ActorOf(FooActor.Props(), "foo-actor");

                // fooActor.Tell(item.ToString(), ActorRefs.Nobody);

                while (true)
                {
                    Console.WriteLine("Please press enter to send event to an actor");
                    Console.ReadLine();
                    fooActor.Tell(1, ActorRefs.Nobody);
                }
                
                // actorSystem.WhenTerminated.Wait();
            }
        }
    }
}