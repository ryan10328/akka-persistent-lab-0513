using System;
using Akka.Actor;
using Akka.Persistence;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace AkkaPersistent
{
    public class FooActor : ReceivePersistentActor
    {
        private int _currentNumber = 0;

        public override string PersistenceId => "foo-actor";

        public FooActor()
        {
            Recover<SetCurrentNumber>(data => { _currentNumber += data.CurrentNumber; });

            Recover<int>(data =>
            {
                _currentNumber += data;
                Console.WriteLine($"Recover(_store): {JsonConvert.SerializeObject(_currentNumber)}");
            });

            Command<int>(data =>
                Persist(data, s =>
                {
                    _currentNumber += data;
                    Console.WriteLine($"Command(_store): {JsonConvert.SerializeObject(_currentNumber)}");
                })
            );

            Command<SetCurrentNumber>(data => { Persist(data, x => { _currentNumber += data.CurrentNumber; }); });
        }

        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new FooActor());
        }
    }

    public class SetCurrentNumber
    {
        public SetCurrentNumber(int currentNumber)
        {
            CurrentNumber = currentNumber;
        }

        public int CurrentNumber { get; set; }
    }
}