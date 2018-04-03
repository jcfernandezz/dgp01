/*
'===============================================================================
'  Generated From - CSharp_dOOdads_View.vbgen
'
'  The supporting base class SqlClientEntity is in the 
'  Architecture directory in "dOOdads".
'===============================================================================
*/

// Generated by MyGeneration Version # (1.4.0.1)

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Specialized;

using MyGeneration.dOOdads;

namespace IntegradorDeGP
{
	public class vwIfcProveedores : SqlClientEntity
	{
		public vwIfcProveedores()
		{
			this.QuerySource = "vwIfcProveedores";
			this.MappingName = "vwIfcProveedores";
		}	
	
        //15/10/13 jcf Crea constructor con cadena de conexi�n
        public vwIfcProveedores(string connstr)
		{
			this.ConnectionString = connstr;
            this.QuerySource = "vwIfcProveedores";
            this.MappingName = "vwIfcProveedores";
		}	

		//=================================================================
		//  	public Function LoadAll() As Boolean
		//=================================================================
		//  Loads all of the records in the database, and sets the currentRow to the first row
		//=================================================================
		public bool LoadAll() 
		{
			return base.Query.Load();
		}
		
		public override void FlushData()
		{
			this._whereClause = null;
			this._aggregateClause = null;
			base.FlushData();
		}
	
		#region Parameters
		protected class Parameters
		{
			
			public static SqlParameter Vendorid
			{
				get
				{
					return new SqlParameter("@Vendorid", SqlDbType.Char, 15);
				}
			}
			
			public static SqlParameter Vendname
			{
				get
				{
					return new SqlParameter("@Vendname", SqlDbType.Char, 65);
				}
			}
			
			public static SqlParameter Vndclsid
			{
				get
				{
					return new SqlParameter("@Vndclsid", SqlDbType.Char, 11);
				}
			}
			
		}
		#endregion	
	
		#region ColumnNames
		public class ColumnNames
		{  
            public const string Vendorid = "vendorid";
            public const string Vendname = "vendname";
            public const string Vndclsid = "vndclsid";

			static public string ToPropertyName(string columnName)
			{
				if(ht == null)
				{
					ht = new Hashtable();
					
					ht[Vendorid] = vwIfcProveedores.PropertyNames.Vendorid;
					ht[Vendname] = vwIfcProveedores.PropertyNames.Vendname;
					ht[Vndclsid] = vwIfcProveedores.PropertyNames.Vndclsid;

				}
				return (string)ht[columnName];
			}

			static private Hashtable ht = null;			 
		}
		#endregion
		
		#region PropertyNames
		public class PropertyNames
		{  
            public const string Vendorid = "Vendorid";
            public const string Vendname = "Vendname";
            public const string Vndclsid = "Vndclsid";

			static public string ToColumnName(string propertyName)
			{
				if(ht == null)
				{
					ht = new Hashtable();
					
					ht[Vendorid] = vwIfcProveedores.ColumnNames.Vendorid;
					ht[Vendname] = vwIfcProveedores.ColumnNames.Vendname;
					ht[Vndclsid] = vwIfcProveedores.ColumnNames.Vndclsid;

				}
				return (string)ht[propertyName];
			}

			static private Hashtable ht = null;			 
		}			 
		#endregion

		#region StringPropertyNames
		public class StringPropertyNames
		{  
            public const string Vendorid = "s_Vendorid";
            public const string Vendname = "s_Vendname";
            public const string Vndclsid = "s_Vndclsid";

		}
		#endregion	
		
		#region Properties
			public virtual string Vendorid
	    {
			get
	        {
				return base.Getstring(ColumnNames.Vendorid);
			}
			set
	        {
				base.Setstring(ColumnNames.Vendorid, value);
			}
		}

		public virtual string Vendname
	    {
			get
	        {
				return base.Getstring(ColumnNames.Vendname);
			}
			set
	        {
				base.Setstring(ColumnNames.Vendname, value);
			}
		}

		public virtual string Vndclsid
	    {
			get
	        {
				return base.Getstring(ColumnNames.Vndclsid);
			}
			set
	        {
				base.Setstring(ColumnNames.Vndclsid, value);
			}
		}


		#endregion
		
		#region String Properties
	
		public virtual string s_Vendorid
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Vendorid) ? string.Empty : base.GetstringAsString(ColumnNames.Vendorid);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Vendorid);
				else
					this.Vendorid = base.SetstringAsString(ColumnNames.Vendorid, value);
			}
		}

		public virtual string s_Vendname
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Vendname) ? string.Empty : base.GetstringAsString(ColumnNames.Vendname);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Vendname);
				else
					this.Vendname = base.SetstringAsString(ColumnNames.Vendname, value);
			}
		}

		public virtual string s_Vndclsid
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Vndclsid) ? string.Empty : base.GetstringAsString(ColumnNames.Vndclsid);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Vndclsid);
				else
					this.Vndclsid = base.SetstringAsString(ColumnNames.Vndclsid, value);
			}
		}


		#endregion			
	
		#region Where Clause
		public class WhereClause
		{
			public WhereClause(BusinessEntity entity)
			{
				this._entity = entity;
			}
			
			public TearOffWhereParameter TearOff
			{
				get
				{
					if(_tearOff == null)
					{
						_tearOff = new TearOffWhereParameter(this);
					}

					return _tearOff;
				}
			}

			#region WhereParameter TearOff's
			public class TearOffWhereParameter
			{
				public TearOffWhereParameter(WhereClause clause)
				{
					this._clause = clause;
				}
				
				
				public WhereParameter Vendorid
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Vendorid, Parameters.Vendorid);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Vendname
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Vendname, Parameters.Vendname);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Vndclsid
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Vndclsid, Parameters.Vndclsid);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}


				private WhereClause _clause;
			}
			#endregion
		
			public WhereParameter Vendorid
		    {
				get
		        {
					if(_Vendorid_W == null)
	        	    {
						_Vendorid_W = TearOff.Vendorid;
					}
					return _Vendorid_W;
				}
			}

			public WhereParameter Vendname
		    {
				get
		        {
					if(_Vendname_W == null)
	        	    {
						_Vendname_W = TearOff.Vendname;
					}
					return _Vendname_W;
				}
			}

			public WhereParameter Vndclsid
		    {
				get
		        {
					if(_Vndclsid_W == null)
	        	    {
						_Vndclsid_W = TearOff.Vndclsid;
					}
					return _Vndclsid_W;
				}
			}

			private WhereParameter _Vendorid_W = null;
			private WhereParameter _Vendname_W = null;
			private WhereParameter _Vndclsid_W = null;

			public void WhereClauseReset()
			{
				_Vendorid_W = null;
				_Vendname_W = null;
				_Vndclsid_W = null;

				this._entity.Query.FlushWhereParameters();

			}
	
			private BusinessEntity _entity;
			private TearOffWhereParameter _tearOff;
			
		}
	
		public WhereClause Where
		{
			get
			{
				if(_whereClause == null)
				{
					_whereClause = new WhereClause(this);
				}
		
				return _whereClause;
			}
		}
		
		private WhereClause _whereClause = null;	
		#endregion
	
		#region Aggregate Clause
		public class AggregateClause
		{
			public AggregateClause(BusinessEntity entity)
			{
				this._entity = entity;
			}
			
			public TearOffAggregateParameter TearOff
			{
				get
				{
					if(_tearOff == null)
					{
						_tearOff = new TearOffAggregateParameter(this);
					}

					return _tearOff;
				}
			}

			#region AggregateParameter TearOff's
			public class TearOffAggregateParameter
			{
				public TearOffAggregateParameter(AggregateClause clause)
				{
					this._clause = clause;
				}
				
				
				public AggregateParameter Vendorid
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Vendorid, Parameters.Vendorid);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Vendname
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Vendname, Parameters.Vendname);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Vndclsid
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Vndclsid, Parameters.Vndclsid);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}


				private AggregateClause _clause;
			}
			#endregion
		
			public AggregateParameter Vendorid
		    {
				get
		        {
					if(_Vendorid_W == null)
	        	    {
						_Vendorid_W = TearOff.Vendorid;
					}
					return _Vendorid_W;
				}
			}

			public AggregateParameter Vendname
		    {
				get
		        {
					if(_Vendname_W == null)
	        	    {
						_Vendname_W = TearOff.Vendname;
					}
					return _Vendname_W;
				}
			}

			public AggregateParameter Vndclsid
		    {
				get
		        {
					if(_Vndclsid_W == null)
	        	    {
						_Vndclsid_W = TearOff.Vndclsid;
					}
					return _Vndclsid_W;
				}
			}

			private AggregateParameter _Vendorid_W = null;
			private AggregateParameter _Vendname_W = null;
			private AggregateParameter _Vndclsid_W = null;

			public void AggregateClauseReset()
			{
				_Vendorid_W = null;
				_Vendname_W = null;
				_Vndclsid_W = null;

				this._entity.Query.FlushAggregateParameters();

			}
	
			private BusinessEntity _entity;
			private TearOffAggregateParameter _tearOff;
			
		}
	
		public AggregateClause Aggregate
		{
			get
			{
				if(_aggregateClause == null)
				{
					_aggregateClause = new AggregateClause(this);
				}
		
				return _aggregateClause;
			}
		}
		
		private AggregateClause _aggregateClause = null;	
		#endregion
	
		protected override IDbCommand GetInsertCommand() 
		{
			return null;
		}
	
		protected override IDbCommand GetUpdateCommand()
		{
			return null;
		}
	
		protected override IDbCommand GetDeleteCommand()
		{
			return null;
		}
	}
}