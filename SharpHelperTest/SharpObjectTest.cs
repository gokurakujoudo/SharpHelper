using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpHelper.Object;
using SharpHelper.Util;

namespace SharpHelperTest {
    [TestClass]
    public class SharpObjectTest {
        private class TestClass : SharpObject {
            public TestClass(int age, string firstName, string lastName) {
                this.Age = age;
                this.FirstName = firstName;
                this.LastName = lastName;
            }

            public TestClass(int age, string name)
            {
                this.Age = age;
                this.FirstName = name;
                this.LastName = name;
            }

            public int Age { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName() => $"{this.FirstName} {this.LastName}";
            public int AgePlus(int add = 10) => this.Age + add;
        }

        [TestMethod]
        public void SharpObjectTestMethod() {
            var o1 = new TestClass(100, "Mike", "Smith");

            // get
            Assert.AreEqual(o1.Get(nameof(o1.Age)), o1.Age);
            Assert.IsNull(o1.Get("x"));

            // getter
            Assert.AreEqual(o1.Getter(nameof(o1.Age))(), o1.Age);
            Assert.IsNull(o1.Getter("x"));

            // set
            const int newAge1 = 100;
            Assert.IsTrue(o1.Set(nameof(o1.Age), newAge1));
            Assert.IsFalse(o1.Set("x", newAge1));
            Assert.AreEqual(o1.Age, newAge1);

            // setter
            const int newAge2 = 200;
            o1.Setter(nameof(o1.Age))(newAge2);
            Assert.AreEqual(o1.Age, newAge2);
            Assert.IsNull(o1.Setter("x"));

            // invoke
            Assert.AreEqual(o1.Invoke(nameof(o1.FullName)), o1.FullName());
            Assert.AreEqual(o1.Invoke(nameof(o1.AgePlus), Type.Missing), o1.AgePlus());
            Assert.AreEqual(o1.Invoke(nameof(o1.AgePlus), 1), o1.AgePlus(1));
            Assert.IsNull(o1.Invoke("f"));

            // invoke by dict
            var arg = new object[,] {{"add", 200}, {"x", null}}.ToDict().ToUpper();
            Assert.AreEqual(o1.InvokeByDict(nameof(o1.AgePlus), arg), o1.AgePlus(200));
            Assert.AreEqual(o1.InvokeByDict(nameof(o1.AgePlus), new Dictionary<string, object>()), o1.AgePlus());
            Assert.AreEqual(o1.InvokeByDict(nameof(o1.AgePlus), null), o1.AgePlus());
            Assert.IsNull(o1.InvokeByDict("f", null));

            // construct by dict
            var paras2 = new object[,] {{"age", 100}, {"name", "Smith"}}.ToDict().ToUpper();
            var o2 = SharpObject.ConstructByDict(typeof(TestClass), paras2);
            Assert.AreEqual(o2.ToString(), @"TestClass[Age: 100 FirstName: Smith LastName: Smith]");

            var paras3 = new object[,] {{"age", 100}, {"FirstName", "Mark"}, {"LastName", "Smith"}}.ToDict().ToUpper();
            var o3 = SharpObject.ConstructByDict(typeof(TestClass), paras3);
            Assert.AreEqual(o3.ToString(), @"TestClass[Age: 100 FirstName: Mark LastName: Smith]");

            Assert.IsNull(SharpObject.ConstructByDict(typeof(TestClass), null));

            // to string
            Assert.AreEqual(o1.ToString(), @"TestClass[Age: 200 FirstName: Mike LastName: Smith]");
        }

        [TestMethod]
        public void SharpCacheTestMethod() {
            var sc = SharpCache.Default;
            var o1 = new TestClass(100, "Mike", "Smith");

            const string name = "var";

            // add
            Assert.IsTrue(sc.AddMember(name, o1));
            Assert.IsFalse(sc.AddMember(name, o1));
            sc.AddOrOverlap(name, o1);

            // list
            Assert.AreEqual(sc.MemberList.Count, 1);
            Assert.AreSame(o1, sc.MemberList[0].Value);
            Assert.AreEqual(name, sc.MemberList[0].Key);

            // get member
            Assert.AreSame(o1, sc.GetMember(name));

            const string prop = nameof(o1.Age);

            // set prop
            Assert.IsTrue(sc.SetMemberProperty(name, prop, 100));
            Assert.IsFalse(sc.SetMemberProperty(name, prop, "abc"));
            Assert.IsFalse(sc.SetMemberProperty(name, "a", 0));
            Assert.IsFalse(sc.SetMemberProperty("x", "a", 0));

            // get prop
            Assert.AreEqual(sc.GetMemberProperty(name, prop), 100);
            Assert.IsNull(sc.GetMemberProperty(name,"a"));
            Assert.IsNull(sc.GetMemberProperty("x","a"));

            const string func = nameof(o1.AgePlus);

            // invoke
            Assert.AreEqual(sc.InvokeMember(name, func, Type.Missing), o1.AgePlus());
            Assert.AreEqual(sc.InvokeMember(name, func, 1), o1.AgePlus(1));
            Assert.IsNull(sc.InvokeMember(name, "f"));
            Assert.IsNull(sc.InvokeMember("x", "f"));

            // remove
            Assert.IsTrue(sc.RemoveMember(name));
            Assert.IsFalse(sc.RemoveMember(name));
        }
    }
}
