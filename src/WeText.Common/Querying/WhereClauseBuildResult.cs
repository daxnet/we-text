
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeText.Common.Querying
{
    /// <summary>
    /// Represents the where clause build result.
    /// </summary>
    public sealed class WhereClauseBuildResult
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets a <c>System.String</c> value which represents the generated
        /// WHERE clause.
        /// </summary>
        public string WhereClause { get; set; }
        /// <summary>
        /// Gets or sets a <c>Dictionary&lt;string, object&gt;</c> instance which contains
        /// the mapping of the parameters and their values.
        /// </summary>
        public Dictionary<string, object> ParameterValues { get; set; }
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>WhereClauseBuildResult</c> class.
        /// </summary>
        public WhereClauseBuildResult() { }
        /// <summary>
        /// Initializes a new instance of <c>WhereClauseBuildResult</c> class.
        /// </summary>
        /// <param name="whereClause">The <c>System.String</c> value which represents the generated
        /// WHERE clause.</param>
        /// <param name="parameterValues">The <c>Dictionary&lt;string, object&gt;</c> instance which contains
        /// the mapping of the parameters and their values.</param>
        public WhereClauseBuildResult(string whereClause, Dictionary<string, object> parameterValues)
        {
            WhereClause = whereClause;
            ParameterValues = parameterValues;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a <c>System.String</c> object which represents the content of the Where Clause
        /// Build Result.
        /// </summary>
        /// <returns>A <c>System.String</c> object which represents the content of the Where Clause
        /// Build Result.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(WhereClause);
            sb.Append(Environment.NewLine);
            ParameterValues.ToList().ForEach(kvp =>
                {
                    sb.Append(string.Format("{0} = [{1}] (Type: {2})", kvp.Key, kvp.Value.ToString(), kvp.Value.GetType().FullName));
                    sb.Append(Environment.NewLine);
                });
            return sb.ToString();
        }
        #endregion
    }
}
