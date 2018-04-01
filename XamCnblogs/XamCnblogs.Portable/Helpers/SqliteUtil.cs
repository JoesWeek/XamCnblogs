using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.Portable.Helpers
{
    public class SqliteUtil
    {
        static SqliteUtil baseSqlite;
        public static SqliteUtil Current
        {
            get { return baseSqlite ?? (baseSqlite = new SqliteUtil()); }
        }
        private static readonly SQLiteAsyncConnection db;
        private static readonly SQLiteConnection dbAsync;
        private static readonly ILog Log;
        static SqliteUtil()
        {
            if (db == null)
                db = DependencyService.Get<ISQLite>().GetAsyncConnection();
            if (dbAsync == null)
                dbAsync = DependencyService.Get<ISQLite>().GetConnection();
            Log = DependencyService.Get<ILog>();
        }
        public async void CreateAllTablesAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    dbAsync.CreateTable<Articles>();
                    dbAsync.CreateTable<KbArticles>();
                    dbAsync.CreateTable<News>();
                    dbAsync.CreateTable<Statuses>();
                    dbAsync.CreateTable<Questions>();
                    dbAsync.CreateTable<QuestionUserInfo>();
                    dbAsync.CreateTable<QuestionAddition>();
                });
            }
            catch (Exception ex)
            {
                Log.SaveLog("CreateAllTablesAsync", ex);
            }
        }

        #region Articles
        public async Task<Model.Articles> QueryArticle(int id)
        {
            return await db.Table<Model.Articles>().Where(a => a.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Model.Articles>> QueryArticles(int pageSize)
        {
            return await db.Table<Model.Articles>().OrderByDescending(a => a.PostDate).Skip(0).Take(pageSize).ToListAsync();
        }
        public async Task<List<Model.Articles>> QueryArticlesByBlogApp(string blogApp)
        {
            return await db.Table<Model.Articles>().Where(a => a.BlogApp == blogApp).OrderByDescending(a => a.PostDate).Skip(0).Take(10).ToListAsync();
        }
        public async Task<List<Model.Articles>> QueryArticlesByDigg(int pageSize)
        {
            return await db.Table<Model.Articles>().Where(a => a.DiggCount > 20).OrderByDescending(a => a.PostDate).Skip(0).Take(pageSize).ToListAsync();
        }
        public async Task UpdateArticles(List<Model.Articles> lists)
        {
            foreach (var item in lists)
            {
                await QueryArticle(item.Id).ContinueWith(async (results) =>
                {
                    if (results.Result == null)
                    {
                        try
                        {
                            await db.InsertAsync(item);
                        }
                        catch (Exception ex)
                        {
                            Log.SaveLog("UpdateArticles.Index", ex);
                        }
                    }
                    else
                    {
                        await UpdateArticle(item);
                    }
                });
            }
        }
        public async Task UpdateArticle(Model.Articles model)
        {
            try
            {
                await db.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                Log.SaveLog("UpdateArticle", ex);
            }
        }
        #endregion

        #region News
        public async Task<Model.News> QueryNew(int id)
        {
            return await db.Table<Model.News>().Where(a => a.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Model.News>> QueryNews(int pageSize)
        {
            return await db.Table<Model.News>().OrderByDescending(a => a.DateAdded).Skip(0).Take(pageSize).ToListAsync();
        }
        public async Task<List<Model.News>> QueryNewsByRecommend(int pageSize)
        {
            return await db.Table<Model.News>().Where(a => a.IsRecommend).OrderByDescending(a => a.DateAdded).Skip(0).Take(pageSize).ToListAsync();
        }
        public async Task<List<Model.News>> QueryNewsByWorkHot(int pageSize, DateTime startdate)
        {
            return await db.Table<Model.News>().Where(a => a.IsHot && a.DateAdded > startdate).OrderByDescending(a => a.DateAdded).Skip(0).Take(pageSize).ToListAsync();
        }
        public async Task UpdateNews(List<Model.News> lists)
        {
            foreach (var item in lists)
            {
                await QueryNew(item.Id).ContinueWith(async (results) =>
                {
                    if (results.Result == null)
                    {
                        try
                        {
                            await db.InsertAsync(item);
                        }
                        catch (Exception ex)
                        {
                            Log.SaveLog("UpdateNews.Insert", ex);
                        }
                    }
                    else
                    {
                        await UpdateNew(item);
                    }
                });
            }
        }
        public async Task UpdateNew(Model.News model)
        {
            try
            {
                await db.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                Log.SaveLog("UpdateNew", ex);
            }
        }
        #endregion

        #region KbArticles
        public async Task<Model.KbArticles> QueryKbArticle(int id)
        {
            return await db.Table<Model.KbArticles>().Where(a => a.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Model.KbArticles>> QueryKbArticles(int pageSize)
        {
            return await db.Table<Model.KbArticles>().OrderByDescending(a => a.DateAdded).Skip(0).Take(pageSize).ToListAsync();
        }
        public async Task UpdateKbArticles(List<Model.KbArticles> lists)
        {
            foreach (var item in lists)
            {
                await QueryKbArticle(item.Id).ContinueWith(async (results) =>
                {
                    if (results.Result == null)
                    {
                        try
                        {
                            await db.InsertAsync(item);
                        }
                        catch (Exception ex)
                        {
                            Log.SaveLog("UpdateKbArticles.Insert", ex);
                        }
                    }
                    else
                    {
                        await UpdateKbArticle(item);
                    }
                });
            }
        }
        public async Task UpdateKbArticle(Model.KbArticles model)
        {
            try
            {
                await db.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                Log.SaveLog("UpdateKbArticle", ex);
            }
        }
        #endregion

        #region Statuses
        public async Task<Model.Statuses> QueryStatus(int id)
        {
            return await db.Table<Model.Statuses>().Where(a => a.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Model.Statuses>> QueryStatuses(int pageSize)
        {
            return await db.Table<Model.Statuses>().OrderByDescending(a => a.DateAdded).Skip(0).Take(pageSize).ToListAsync();
        }
        public async Task UpdateStatuses(List<Model.Statuses> lists)
        {
            foreach (var item in lists)
            {
                await QueryStatus(item.Id).ContinueWith(async (results) =>
                {
                    if (results.Result == null)
                    {
                        try
                        {
                            await db.InsertAsync(item);
                        }
                        catch (Exception ex)
                        {
                            Log.SaveLog("UpdateStatuses.Insert", ex);
                        }
                    }
                    else
                    {
                        await UpdateStatus(item);
                    }
                });
            }
        }
        public async Task UpdateStatus(Model.Statuses model)
        {
            try
            {
                await db.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                Log.SaveLog("UpdateStatus", ex);
            }
        }
        #endregion

        #region Questions
        public async Task<Model.Questions> QueryQuestion(int id)
        {
            var model = await db.Table<Model.Questions>().Where(a => a.Qid == id).FirstOrDefaultAsync();
            if (model != null && model.UserInfoID > 0)
            {
                model.QuestionUserInfo = await QueryQuestionUserInfo(model.UserInfoID);
            }
            if (model != null && model.AdditionID > 0)
            {
                model.Addition = await QueryAddition(model.AdditionID);
            }
            return model;
        }
        public async Task<List<Model.Questions>> QueryQuestions(int pageSize)
        {
            var list = await db.Table<Model.Questions>().OrderByDescending(a => a.DateAdded).Skip(0).Take(pageSize).ToListAsync();
            list.ForEach(async (q) =>
            {
                q.QuestionUserInfo = await QueryQuestionUserInfo(q.UserInfoID);
                q.Addition = await QueryAddition(q.AdditionID);
            });
            return list;
        }
        public async Task<List<Model.Questions>> QueryQuestionsByType(int type, int pageSize)
        {
            List<Model.Questions> list = new List<Model.Questions>();
            switch (type)
            {
                case 0:
                    list = await db.Table<Model.Questions>().OrderByDescending(a => a.DateAdded).Skip(0).Take(pageSize).ToListAsync();
                    break;
                case 1:
                    list = await db.Table<Model.Questions>().Where(a => a.Award >= 50).OrderByDescending(a => a.DateAdded).Skip(0).Take(pageSize).ToListAsync();
                    break;
                case 2:
                    list = await db.Table<Model.Questions>().Where(a => a.AnswerCount == 0).OrderByDescending(a => a.DateAdded).Skip(0).Take(pageSize).ToListAsync();
                    break;
                case 3:
                    list = await db.Table<Model.Questions>().Where(a => a.DealFlag == 1).OrderByDescending(a => a.DateAdded).Skip(0).Take(pageSize).ToListAsync();
                    break;
                case 4:
                    return list;
            }
            list.ForEach(async (q) =>
            {
                q.QuestionUserInfo = await QueryQuestionUserInfo(q.UserInfoID);
                q.Addition = await QueryAddition(q.AdditionID);
            });
            return list;
        }
        public async Task UpdateQuestions(List<Model.Questions> lists)
        {
            foreach (var item in lists)
            {
                await UpdateQuestion(item);
            }
        }
        public async Task UpdateQuestion(Model.Questions model)
        {
            model.UserInfoID = model.QuestionUserInfo.UserID;
            UpdateQuestionUserInfo(model.QuestionUserInfo);
            if (model.Addition != null)
            {
                model.AdditionID = model.Addition.QID;
                UpdateAddition(model.Addition);
            }
            await QueryQuestion(model.Qid).ContinueWith(async (results) =>
            {
                if (results.Result == null)
                {
                    try
                    {
                        await db.InsertAsync(model);
                    }
                    catch (Exception ex)
                    {
                        Log.SaveLog("UpdateQuestion.Insert", ex);
                    }
                }
                else
                {
                    try
                    {
                        await db.UpdateAsync(model);
                    }
                    catch (Exception ex)
                    {
                        Log.SaveLog("UpdateQuestion.Update", ex);
                    }
                }
            });
        }
        public async Task DeleteQuestions()
        {
            try
            {
                await db.DeleteAsync(await db.Table<Model.Questions>().ToListAsync());
            }
            catch (Exception ex)
            {
                Log.SaveLog("DeleteQuestions", ex);
            }
        }
        #endregion

        #region QuestionUserInfo
        public async Task<Model.QuestionUserInfo> QueryQuestionUserInfo(int id)
        {
            return await db.Table<Model.QuestionUserInfo>().Where(a => a.UserID == id).FirstOrDefaultAsync();
        }
        public async void UpdateQuestionUserInfo(Model.QuestionUserInfo model)
        {
            await QueryQuestionUserInfo(model.UserID).ContinueWith(async (results) =>
            {
                if (results.Result == null)
                {
                    try
                    {
                        await db.InsertAsync(model);
                    }
                    catch (Exception ex)
                    {
                        Log.SaveLog("UpdateQuestionUserInfo.Insert", ex);
                    }
                }
                else
                {
                    try
                    {
                        await db.UpdateAsync(model);
                    }
                    catch (Exception ex)
                    {
                        Log.SaveLog("UpdateQuestionUserInfo.Update", ex);
                    }
                }
            });
        }
        #endregion

        #region QuestionAddition
        public async Task<Model.QuestionAddition> QueryAddition(int id)
        {
            return await db.Table<Model.QuestionAddition>().Where(a => a.QID == id).FirstOrDefaultAsync();
        }
        public async void UpdateAddition(Model.QuestionAddition model)
        {
            await QueryAddition(model.QID).ContinueWith(async (results) =>
            {
                if (results.Result == null)
                {
                    try
                    {
                        await db.InsertAsync(model);
                    }
                    catch (Exception ex)
                    {
                        Log.SaveLog("UpdateAddition.Insert", ex);
                    }
                }
                else
                {
                    try
                    {
                        await db.UpdateAsync(model);
                    }
                    catch (Exception ex)
                    {
                        Log.SaveLog("UpdateAddition.Update", ex);
                    }
                }
            });
        }
        #endregion
    }
}
