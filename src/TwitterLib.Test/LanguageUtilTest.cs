using TwitterLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TwitterLib.Test
{
    [TestClass]
    public class LanguageUtilTest
    {


        [TestMethod()]
        public void DetectLanguageTest()
        {
            var text = "ez itt egy szöveges üzenet";

            int words;
            string lang1, lang2;

            Assert.IsTrue(LanguageUtil.DetectLanguage(text, out words, out lang1, out lang2));
            Assert.AreEqual("hu", lang1);
            Assert.AreEqual("??", lang2);
            Assert.AreEqual(words, 5);
        }

        [TestMethod()]
        public void DetectLanguageRemoveHashtagTest()
        {
            var text = "ez itt egy szöveges #hashtag üzenet";

            int words;
            string lang1, lang2;

            Assert.IsTrue(LanguageUtil.DetectLanguage(text, out words, out lang1, out lang2));
            Assert.AreEqual("hu", lang1);
            Assert.AreEqual("??", lang2);
            Assert.AreEqual(5, words);
        }

        [TestMethod()]
        public void DetectLanguageRemoveUserMentionTest()
        {
            var text = "ez itt egy szöveges @username üzenet";

            int words;
            string lang1, lang2;

            Assert.IsTrue(LanguageUtil.DetectLanguage(text, out words, out lang1, out lang2));
            Assert.AreEqual("hu", lang1);
            Assert.AreEqual("??", lang2);
            Assert.AreEqual(5, words);
        }

        [TestMethod()]
        public void DetectLanguageRemoveUrlTest()
        {
            var text = "ez itt egy szöveges http://valami.com üzenet";

            int words;
            string lang1, lang2;

            Assert.IsTrue(LanguageUtil.DetectLanguage(text, out words, out lang1, out lang2));
            Assert.AreEqual("hu", lang1);
            Assert.AreEqual("??", lang2);
            Assert.AreEqual(5, words);
        }

        [TestMethod()]
        public void DetectLanguageLongTextTest()
        {
            var text = new String('q', 65536);

            int words;
            string lang1, lang2;

            Assert.IsFalse(LanguageUtil.DetectLanguage(text, out words, out lang1, out lang2));
            //Assert.AreEqual("hu", lang1);
            //Assert.AreEqual("??", lang2);
            //Assert.AreEqual(5, words);
        }

    }
}
