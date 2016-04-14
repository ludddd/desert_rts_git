using NUnit.Framework;

namespace utils
{
    [TestFixture]
    class Test_PathUtils
    {
        [TestCase(".", ".\\a", "a")]
        [TestCase(".", ".\\a\\b\\c", "a\\b\\c")]
        [TestCase(".\\a", ".", "..")]
        [TestCase(".\\a\\b\\c", ".", "..\\..\\..")]
        [TestCase(".\\a", ".\\b", "..\\b")]
        [TestCase(".\\a", ".\\a\\b", "b")]
        [TestCase(".\\a\\b", ".\\a\\c", "..\\c")]
        [TestCase("c:\\a\\b", "c:\\a\\c", "..\\c")]
        [TestCase("c:\\a", "d:\\a", null)]
        public void RelativePath(string from, string to, string result)
        {
            Assert.AreEqual(result, utils.PathUtils.GetRelativePath(from, to));
        }
    }
}

