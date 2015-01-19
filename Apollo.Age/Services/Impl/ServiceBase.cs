namespace Apollo.Age.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Configurations;
    using Apollo.Age.Entities;
    using Fanex.Common;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Service base.
    /// </summary>
    public class ServiceBase
    {
        private readonly IDynamicRepository _dynamicRepository;

        public ServiceBase()
        {
            _dynamicRepository = new DynamicRepository();
        }

        /// <summary>
        ///     Get Report Data with Criteria.
        /// </summary>
        /// <typeparam name="T">Type of Object return.</typeparam>
        /// <param name="criteria">Criteria to Query DB.</param>
        /// <returns>Enumerable of Type T.</returns>
        protected IEnumerable<T> GetListData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _dynamicRepository.Fetch<T>(criteria);
                }
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw;
            }

            return new List<T>();
        }

        /// <summary>
        /// Revert Commission Group Name.
        /// </summary>
        /// <param name="commGroupName"> Input commission group name normally (a,b,c,d).</param>
        /// <returns>Return commission group name that need to revert(d,c,b,a).</returns>
        protected string RevertCommGroupName(string commGroupName)
        {
            switch (commGroupName.ToLower())
            {
                case "a":
                    return "D";

                case "b":
                    return "C";

                case "c":
                    return "B";

                case "d":
                    return "A";

                default:
                    return "All";
            }
        }

        /// <summary>
        /// Get Peak hours for prevent action, format peak hour in config file: DAYINWEEK-HOUR.
        /// </summary>
        /// <returns>
        /// Peak Hours list.
        /// </returns>
        protected IList<PeakHour> GetPeakHours()
        {
            IList<PeakHour> peakHours = new List<PeakHour>();
            var peakHoursString = AppSettings.PeakHours;
            if (!string.IsNullOrEmpty(peakHoursString))
            {
                var peakHourArray = peakHoursString.Split(';');
                foreach (string peakHourSetting in peakHourArray)
                {
                    if (!string.IsNullOrEmpty(peakHourSetting))
                    {
                        var peakHourItem = peakHourSetting.Split('>');
                        var peakHour = new PeakHour
                        {
                            FromDayOfWeek = (DayOfWeek)Converter.ToInt(peakHourItem[0].Split(':')[0]),
                            FromHour = Converter.ToInt(peakHourItem[0].Split(':')[1]),
                            ToDayOfWeek = (DayOfWeek)Converter.ToInt(peakHourItem[1].Split(':')[0]),
                            ToHour = Converter.ToInt(peakHourItem[1].Split(':')[1])
                        };

                        peakHours.Add(peakHour);
                    }
                }
            }

            return peakHours;
        }

        /// <summary>
        /// Executes the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Result execute criteria.</returns>
        /// <exception cref="System.InvalidOperationException">Invalid Criteria.</exception>
        protected Result Execute(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid Criteria");
                }

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    objectDb.ExecuteNonQuery(param);

                    return new Result { ErrorCode = Converter.ToInt(objectDb.GetParameterValue("ErrCode")), ErrorMsg = Converter.ToString2(objectDb.GetParameterValue("ErrMsg")) };
                }
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw;
            }
        }
    }
}