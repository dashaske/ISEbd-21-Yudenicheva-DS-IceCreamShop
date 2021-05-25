using System;
using System.Collections.Generic;

namespace IceCreamShopBusinessLogic.ViewModel
{
    public class PageViewModel
    {
        public int PageNumber { get; private set; }

        public int TotalPages { get; private set; }

        public List<MessageInfoViewModel> Messages { get; private set; }

        public int Count { get; private set; }

        public int PageSize { get; private set; }

        public PageViewModel(int count, int pageNumber, int pageSize, List<MessageInfoViewModel> messages)
        {
            PageNumber = pageNumber;
            Count = count;
            Messages = messages;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(Count / (double)pageSize);
        }

        public bool HasPreviousPage
        {
            get { return PageNumber > 1; }
        }

        public bool HasNextPage
        {
            get { return PageNumber < TotalPages; }
        }
    }
}
