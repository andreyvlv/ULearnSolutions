using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class ParsingTask
	{
        /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
        /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
        /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines
                .Select(x => x.SplitSlideLine())
                .Where(x => x != null)
                .ToDictionary(a => a.SlideId, b => b);
        }

        /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
        /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
        /// Такой словарь можно получить методом ParseSlideRecords</param>
        /// <returns>Список информации о посещениях</returns>
        /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            return lines.Skip(1).Select(x => x.SplitVisitRecord(slides));
        }
    }

    public static class ParserExtensions
    {
        static readonly Dictionary<string, SlideType> converter = new Dictionary<string, SlideType>()
        {
            { "theory", SlideType.Theory },
            { "quiz", SlideType.Quiz },
            { "exercise", SlideType.Exercise }
        };

        public static SlideRecord SplitSlideLine(this string s)
        {
            var slide = s.Split(';');
            try
            {
                return new SlideRecord(int.Parse(slide[0]), converter[slide[1]], slide[2]);
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static VisitRecord SplitVisitRecord(this string s, IDictionary<int, SlideRecord> slides)
        {
            var format = "yyyy-MM-dd HH:mm:ss";
            var provider = System.Globalization.CultureInfo.InvariantCulture;
            var visit = s.Split(';');
            try
            {
                var date = string.Join(" ", visit[2], visit[3]);
                var visitDate = DateTime.ParseExact(date, format, provider);
                var slideType = slides[int.Parse(visit[1])].SlideType;
                return new VisitRecord(int.Parse(visit[0]), int.Parse(visit[1]), visitDate, slideType);
            }
            catch (IndexOutOfRangeException)
            {
                throw new FormatException($"Wrong line [{s}]");
            }
            catch (KeyNotFoundException)
            {
                throw new FormatException($"Wrong line [{s}]");
            }
            catch (FormatException)
            {
                throw new FormatException($"Wrong line [{s}]");
            }
        }
    }
}