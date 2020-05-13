using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Persistence;
using Akka.Util.Internal;

namespace AkkaPersistent
{
    public class FooActor : ReceivePersistentActor
    {
        private int _currentNumber = 0;

        public override string PersistenceId => "foo-actor";

        public FooActor()
        {
            Recover<int>(data =>
            {
                Console.WriteLine($"Recover(currentNumber): {_currentNumber}");
                _currentNumber += data;
            });

            Command<int>(data =>
                Persist(data, s =>
                {
                    Console.WriteLine($"Command(currentNumber): {_currentNumber}");
                    _currentNumber += data;
                })
            );
        }

        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new FooActor());
        }
    }
}