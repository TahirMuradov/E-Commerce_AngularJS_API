using Shop.Application.DTOs;

namespace Shop.Application.PaginationHelper
{
    public class PageHelper<T>
    {
        public PageDTO CreatePage(List<T> listData, int page)
        {
            PageDTO pageDTO = new PageDTO();
            pageDTO.PageSize = (int)Math.Ceiling((decimal)listData.Count / pageDTO.MaxProductCount);
            if (page == 0 || pageDTO.PageSize < page) page = 1;
            List<T> pageDatas;
            pageDatas = listData.Skip((page - 1) * pageDTO.MaxProductCount).Take(pageDTO.MaxProductCount).ToList();
            pageDTO.ActivePage = page;
            listData.Clear();
            listData.AddRange(pageDatas);
            return pageDTO;
        }
    }
}
