using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChessLib;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestConstructCreate()
        {
            ChessLib.ChessClass chess = new ChessClass();
            Assert.AreEqual("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", chess.fen);
        }
    }
}
