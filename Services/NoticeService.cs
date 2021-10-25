using MessingSystem.Data;
using MessingSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Services
{
    public class NoticeService : INoticeService
    {
        private AppDbContext dbContext;

        public NoticeService(AppDbContext context)
        {
            dbContext = context;
        }

        public void AddNotice(Notice notice)
        {
            dbContext.Notices.Add(notice);
            dbContext.SaveChanges();
        }

        public Notice GetNotice(int noticeId)
        {
            return dbContext.Notices.Where(it => it.Id == noticeId).FirstOrDefault();
        }

        public IList<Notice> GetNotices(bool visibleInDashboardOnly = false)
        {
            var notices =  dbContext.Notices.ToList();

            if (visibleInDashboardOnly)
                notices = notices.Where(it => it.IsVisible).ToList();

            return notices;
        }

        public void UpdateNotice(Notice notice)
        {
            dbContext.Notices.Update(notice);
            dbContext.SaveChanges();
        }

        public void DeleteNotice(Notice notice)
        {
            dbContext.Notices.Remove(notice);
            dbContext.SaveChanges();
        }
    }
}
