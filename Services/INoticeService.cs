using MessingSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Services
{
    public interface INoticeService
    {
        IList<Notice> GetNotices(bool visibleInDashboard = false);

        Notice GetNotice(int noticeId);

        void AddNotice(Notice notice);

        void UpdateNotice(Notice notice);

        void DeleteNotice(Notice notice);
    }
}
