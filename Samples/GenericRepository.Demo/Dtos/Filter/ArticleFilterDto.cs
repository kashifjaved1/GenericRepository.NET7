using AutoFilterer.Attributes;
using AutoFilterer.Types;

namespace GenericRepository.Demo.Dtos.Filter
{
    [PossibleSortings("Topic", "Surname", "CreationDate", "ModificationDate", "DeletionDate")]
    public class ArticleFilterDto : PaginationFilterBase
    {
        public ArticleFilterDto()
        {
            Sort = "Topic";
            SortBy = AutoFilterer.Enums.Sorting.Descending;
        }

        [ToLowerContainsComparison]
        public string Topic { get; set; }

        [ToLowerContainsComparison]
        public string Content { get; set; }

        public Range<DateTime> CreationDate { get; set; }

        public Range<DateTime> ModificationDate { get; set; }
        public Range<DateTime> DeletionDate { get; set; }
    }
}
