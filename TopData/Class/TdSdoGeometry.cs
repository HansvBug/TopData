/// <summary>
/// This Class is used for showing the sdo_geometry field in a datagridview
/// sdo_geometry is a user defined type
/// </summary>
namespace TopData
{
    using System;
    using System.Text;

    // using Oracle.DataAccess.Client;
    using Oracle.ManagedDataAccess.Client;

    // using Oracle.DataAccess.Types;

/*        [Serializable]
        [OracleCustomTypeMappingAttribute("MDSYS.SDO_GEOMETRY")]
        public class TdSdoGeometry : OracleCustomTypeBase<SdoGeometry>
        {
            [OracleObjectMappingAttribute(0)]
            public decimal? SdoGtype { get; set; }

            [OracleObjectMappingAttribute(1)]
            public decimal? SdoSRID { get; set; }

            [OracleObjectMappingAttribute(2)]
            public SdoPoint SdoPoint { get; set; }

            [OracleObjectMappingAttribute(3)]
            public decimal[] SdoElemInfo { get; set; }

            [OracleObjectMappingAttribute(4)]
            public decimal[] SdoOrdinates { get; set; }

            public int SdoGtypeAsInt
            {
                get
                {
                    return System.Convert.ToInt32(this.SdoGtype);
                }
            }

            public int SdoSRIDAsInt
            {
                get
                {
                    return System.Convert.ToInt32(this.SdoSRID);
                }

                set
                {
                    this.SdoSRID = System.Convert.ToDecimal(value);
                }
            }

            public int[] ElemArrayOfInts
            {
                get
                {
                    int[] elems = null;
                    if (this.SdoElemInfo != null)
                    {
                        elems = new int[this.SdoElemInfo.Length];
                        for (int k = 0; k < this.SdoElemInfo.Length; k++)
                        {
                            elems[k] = System.Convert.ToInt32(this.SdoElemInfo[k]);
                        }
                    }

                    return elems;
                }

                set
                {
                    if (value != null)
                    {
                        int c = value.GetLength(0);
                        this.SdoElemInfo = new decimal[c];

                        for (int k = 0; k < c; k++)
                        {
                            this.SdoElemInfo[k] = System.Convert.ToDecimal(value[k]);
                        }
                    }
                    else
                    {
                        this.SdoElemInfo = null;
                    }
                }
            }

            public double[] OrdinatesArrayOfDoubles
            {
                get
                {
                    double[] elems = null;
                    if (this.SdoOrdinates != null)
                    {
                        elems = new double[this.SdoOrdinates.Length];
                        for (int k = 0; k < this.SdoOrdinates.Length; k++)
                        {
                            elems[k] = System.Convert.ToDouble(this.SdoOrdinates[k]);
                        }
                    }

                    return elems;
                }

                set
                {
                    if (value != null)
                    {
                        int c = value.GetLength(0);
                        this.SdoOrdinates = new decimal[c];
                        for (int k = 0; k < c; k++)
                        {
                            this.SdoOrdinates[k] = System.Convert.ToDecimal(value[k]);
                        }
                    }
                    else
                    {
                        this.SdoOrdinates = null;
                    }
                }
            }

            public int Dimensionality { get; set; }

            public int LRS { get; set; }

            public int GeometryType { get; set; }

            public string AsText
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("MDSYS.SDO_GEOMETRY(");
                    sb.Append((this.SdoGtype != null) ? this.SdoGtype.ToString() : "null");
                    sb.Append(",");
                    sb.Append((this.SdoSRID != null) ? this.SdoSRID.ToString() : "null");
                    sb.Append(",");

                    // begin point
                    if (this.SdoPoint != null)
                    {
                        sb.Append("MDSYS.SDO_POINT_TYPE(");
                        sb.Append(string.Format(
                            "{0:#.##########},{1:#.##########}{2}{3:#.##########}",
                            this.SdoPoint.X,
                            this.SdoPoint.Y,
                            (this.SdoPoint.Z == null) ? null : ",",
                            this.SdoPoint.Z).Trim());
                        sb.Append(")");
                    }
                    else
                    {
                        sb.Append("null");
                    }

                    sb.Append(",");

                    // begin element array
                    if (this.SdoElemInfo != null)
                    {
                        sb.Append("MDSYS.SDO_ELEM_INFO_ARRAY(");
                        for (int i = 0; i < this.SdoElemInfo.Length; i++)
                        {
                            sb.Append(string.Format("{0}", this.SdoElemInfo[i]));
                            if (i < (this.SdoElemInfo.Length - 1))
                            {
                                sb.Append(",");
                            }
                        }

                        sb.Append(")");
                    }
                    else
                    {
                        sb.Append("null");
                    }

                    sb.Append(",");

                    // begin ordinates array
                    if (this.SdoOrdinates != null)
                    {
                        sb.Append("MDSYS.SDO_ORDINATE_ARRAY(");
                        for (int i = 0; i < this.SdoOrdinates.Length; i++)
                        {
                            sb.Append(string.Format("{0:#.##########}", this.SdoOrdinates[i]));
                            if (i < (this.SdoOrdinates.Length - 1))
                            {
                                sb.Append(",");
                            }
                        }

                        sb.Append(")");
                    }
                    else
                    {
                        sb.Append("null");
                    }

                    sb.Append(")");

                    return sb.ToString();
                }
            }

            public override void MapFromCustomObject()
            {
                this.SetValue((int)OracleObjectColumns.SDO_GTYPE, this.SdoGtype);
                this.SetValue((int)OracleObjectColumns.SDO_SRID, this.SdoSRID);
                this.SetValue((int)OracleObjectColumns.SDO_POINT, this.SdoPoint);
                this.SetValue((int)OracleObjectColumns.SDO_ELEM_INFO, this.SdoElemInfo);
                this.SetValue((int)OracleObjectColumns.SDO_ORDINATES, this.SdoOrdinates);
            }

            public override void MapToCustomObject()
            {
                this.SdoGtype = this.GetValue<decimal?>((int)OracleObjectColumns.SDO_GTYPE);
                this.SdoSRID = this.GetValue<decimal?>((int)OracleObjectColumns.SDO_SRID);
                this.SdoPoint = this.GetValue<SdoPoint>((int)OracleObjectColumns.SDO_POINT);
                this.SdoElemInfo = this.GetValue<decimal[]>((int)OracleObjectColumns.SDO_ELEM_INFO);
                this.SdoOrdinates = this.GetValue<decimal[]>((int)OracleObjectColumns.SDO_ORDINATES);
            }

            public int PropertiesFromGTYPE()
            {
                if ((int)this.SdoGtype != 0)
                {
                    int v = (int)this.SdoGtype;
                    int dim = v / 1000;
                    this.Dimensionality = dim;
                    v -= dim * 1000;
                    int lrsDim = v / 100;
                    this.LRS = lrsDim;
                    v -= lrsDim * 100;
                    this.GeometryType = v;
                    return (this.Dimensionality * 1000) + (this.LRS * 100) + this.GeometryType;
                }
                else
                {
                    return 0;
                }
            }

            public int PropertiesToGTYPE()
            {
                int v = this.Dimensionality * 1000;
                v = v + (this.LRS * 100);
                v = v + this.GeometryType;

                this.SdoGtype = System.Convert.ToDecimal(v);

                return v;
            }

            public override string ToString()
            {
                return this.AsText;
            }
        }

        [OracleCustomTypeMappingAttribute("MDSYS.SDO_ELEM_INFO_ARRAY")]
        public class ElemArrayFactory : OracleArrayTypeFactoryBase<decimal>
        {
        }

        public abstract class OracleArrayTypeFactoryBase<T> : IOracleArrayTypeFactory
        {
            public Array CreateArray(int numElems)
            {
                return new T[numElems];
            }

            public Array CreateStatusArray(int numElems)
            {
                return new OracleUdtStatus[numElems];
            }
        }

        [Serializable]
        public abstract class OracleCustomTypeBase<T> : INullable, IOracleCustomType, IOracleCustomTypeFactory
        where T : OracleCustomTypeBase<T>, new()
        {
            private static readonly string errorMessageHead = "Error converting Oracle User Defined Type to .Net Type " + typeof(T).ToString() + ", oracle column is null, failed to map to . NET valuetype, column ";
            [NonSerialized]
            private OracleConnection connection;
            private IntPtr udtHandle;
            private bool isNull;

            public static T Null
            {
                get
                {
                    T t = new T
                    {
                        isNull = true
                    };
                    return t;
                }
            }

            public virtual bool IsNull
            {
                get
                {
                    return this.isNull;
                }
            }

            public IOracleCustomType CreateObject()
            {
                return new T();
            }

            public void FromCustomObject(OracleConnection connection, IntPtr udtHandle)
            {
                this.SetConnectionAndPointer(connection, udtHandle);
                this.MapFromCustomObject();
            }

            public void ToCustomObject(OracleConnection connection, IntPtr udtHandle)
            {
                this.SetConnectionAndPointer(connection, udtHandle);
                this.MapToCustomObject();
            }

            public abstract void MapFromCustomObject();

            public abstract void MapToCustomObject();

            protected void SetConnectionAndPointer(OracleConnection connection, IntPtr udtHandle)
            {
                this.connection = connection;
                this.udtHandle = udtHandle;
            }

            protected void SetValue(string oracleColumnName, object value)
            {
                if (value != null)
                {
                    OracleUdt.SetValue(this.connection, this.udtHandle, oracleColumnName, value);
                }
            }

            protected void SetValue(int oracleColumnId, object value)
            {
                if (value != null)
                {
                    OracleUdt.SetValue(this.connection, this.udtHandle, oracleColumnId, value);
                }
            }

            protected U GetValue<U>(string oracleColumnName)
            {
                if (OracleUdt.IsDBNull(this.connection, this.udtHandle, oracleColumnName))
                {
                    if (default(U) is ValueType)
                    {
                        throw new Exception(errorMessageHead + oracleColumnName.ToString() + " of value type " + typeof(U).ToString());
                    }
                    else
                    {
                        return default(U);
                    }
                }
                else
                {
                    return (U)OracleUdt.GetValue(this.connection, this.udtHandle, oracleColumnName);
                }
            }

            protected U GetValue<U>(int oracleColumnId)
            {
                if (OracleUdt.IsDBNull(this.connection, this.udtHandle, oracleColumnId))
                {
                    if (default(U) is ValueType)
                    {
                        throw new Exception(errorMessageHead + oracleColumnId.ToString() + " of value type " + typeof(U).ToString());
                    }
                    else
                    {
                        return default(U);
                    }
                }
                else
                {
                    return (U)OracleUdt.GetValue(this.connection, this.udtHandle, oracleColumnId);
                }
            }
        }

        public enum OracleObjectColumns
        {
            SDO_GTYPE,
            SDO_SRID,
            SDO_POINT,
            SDO_ELEM_INFO,
            SDO_ORDINATES
        }

        [OracleCustomTypeMappingAttribute("MDSYS.SDO_ORDINATE_ARRAY")]
        public class OrdinatesArrayFactory : OracleArrayTypeFactoryBase<decimal>
        {
        }


        [Serializable]
        public static class SdoGeometryTypes
        {
            // Oracle Documentation for SDO_ETYPE - SIMPLE
            // Point//Line//Polygon//exterior counterclockwise - polygon ring = 1003//interior clockwise  polygon ring = 2003
            public enum ETYPE_SIMPLE
            {
                POINT = 1,
                LINE = 2,
                POLYGON = 3,
                POLYGON_EXTERIOR = 1003,
                POLYGON_INTERIOR = 2003
            }

            // Oracle Documentation for SDO_ETYPE - COMPOUND
            // 1005: exterior polygon ring (must be specified in counterclockwise order)
            // 2005: interior polygon ring (must be specified in clockwise order)
            public enum ETYPE_COMPOUND
            {
                FOURDIGIT = 4,
                POLYGON_EXTERIOR = 1005,
                POLYGON_INTERIOR = 2005
            }

            // Oracle Documentation for SDO_GTYPE.
            // This represents the last two digits in a GTYPE, where the first item is dimension(ality) and the second is LRS
            public enum GTYPE
            {
                UNKNOWN_GEOMETRY = 00,
                POINT = 01,
                LINE = 02,
                CURVE = 02,
                POLYGON = 03,
                COLLECTION = 04,
                MULTIPOINT = 05,
                MULTILINE = 06,
                MULTICURVE = 06,
                MULTIPOLYGON = 07
            }

            public enum DIMENSION
            {
                DIM2D = 2,
                DIM3D = 3,
                LRS_DIM3 = 3,
                LRS_DIM4 = 4
            }
        }

        [Serializable]
        [OracleCustomTypeMappingAttribute("MDSYS.SDO_POINT_TYPE")]
        public class SdoPoint : OracleCustomTypeBase<SdoPoint>
        {
            [OracleObjectMappingAttribute("X")]
            public decimal? X { get; set; }

            [OracleObjectMappingAttribute("Y")]
            public decimal? Y { get; set; }

            [OracleObjectMappingAttribute("Z")]
            public decimal? Z { get; set; }

            public double? XD
            {
                get { return System.Convert.ToDouble(this.X); }
                set { this.X = System.Convert.ToDecimal(value); }
            }

            public double? YD
            {
                get { return System.Convert.ToDouble(this.Y); }
                set { this.Y = System.Convert.ToDecimal(value); }
            }

            public double? ZD
            {
                get { return System.Convert.ToDouble(this.Z); }
                set { this.Z = System.Convert.ToDecimal(value); }
            }

            public override void MapFromCustomObject()
            {
                this.SetValue("X", this.X);
                this.SetValue("Y", this.Y);
                this.SetValue("Z", this.Z);
            }

            public override void MapToCustomObject()
            {
                this.X = this.GetValue<decimal?>("X");
                this.Y = this.GetValue<decimal?>("Y");
                this.Z = this.GetValue<decimal?>("Z");
            }
        }*/
}