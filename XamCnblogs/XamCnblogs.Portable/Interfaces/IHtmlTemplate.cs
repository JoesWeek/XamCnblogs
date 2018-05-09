using System;
using System.Collections.Generic;
using System.Text;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IHtmlTemplate
    {
        string GetArticlesTemplate();
        string GetKbArticlesTemplate();
        string GetNewsTemplate();
        string GetQuestionsTemplate();
        string GetAnswersTemplate();
        string GetStatusesTemplate();
    }
}
