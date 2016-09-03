﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using Zhuang.Data.Utility;

namespace Zhuang.Data.Test
{
    [TestClass]
    public class ParameterParserTest
    {
        DbAccessor _dba = DbAccessor.Get();


        [TestMethod]
        public void EvnValParameterTest()
        {
            var date = _dba.ExecuteScalar<string>("select '{{date}}'");
            Console.WriteLine(date);
            Assert.AreEqual<string>(DateTime.Now.ToString("yyyy-MM-dd"), date);

            var datetime = _dba.ExecuteScalar<string>("select '{{datetime:yyyyMMddHHmmss}}'");
            Console.WriteLine(datetime);
            Assert.AreEqual<string>(DateTime.Now.ToString("yyyyMMddHHmmss"), datetime);

            var guid= _dba.ExecuteScalar<string>("select '{{guid}}'");
            Console.WriteLine(guid);
            Assert.AreEqual<int>(Guid.NewGuid().ToString().Length, guid.Length);

        }

        [TestMethod]
        public void DynamicClauseTest()
        {
            var dt = _dba.QueryDataTable("select count(*) from sys_product where {? 1=#AA# }", new { AA=1});
            Console.WriteLine(DataTableUtil.ToString(dt));

            var dt2 = _dba.QueryDataTable("select count(*) from sys_product where {?? 1=#AA# }", new { AA = 1 });
            Console.WriteLine(DataTableUtil.ToString(dt2));


        }

        [TestMethod]
        public void ValuedParameterTest()
        {
            var v = _dba.ExecuteScalar<string>("select '$P$'",new { P="zwb"});
            Console.WriteLine(v);
            Assert.AreEqual<string>("zwb", v);
        }

        [TestMethod]
        public void NormalParameterTest()
        {
            var v = _dba.ExecuteScalar<string>("select #P#", new { P = "zwb" });
            Console.WriteLine(v);
            Assert.AreEqual<string>("zwb", v);

            var v2 = _dba.ExecuteScalar<string>("select @P", new { P = "zwb" });
            Console.WriteLine(v2);
            Assert.AreEqual<string>("zwb", v2);
        }

    }
}
