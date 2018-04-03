using System;
using FakePI.Lib.Entities;
using Newtonsoft.Json;

namespace FakePI.Lib.Test.Fakes
{
    public class SimpleEntity: Entity
    {
        public string SomeString { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}