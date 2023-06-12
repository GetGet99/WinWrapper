using System.Runtime.InteropServices;

namespace WinWrapper.Windowing.AdditionalPInvoke;


// PropertyKey Structure
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct PropertyKey
{
    private Guid formatId;    // Unique GUID for property
    private Int32 propertyId; // Property identifier (PID)

    public Guid FormatId
    {
        get
        {
            return formatId;
        }
    }

    public Int32 PropertyId
    {
        get
        {
            return propertyId;
        }
    }

    public PropertyKey(Guid formatId, Int32 propertyId)
    {
        this.formatId = formatId;
        this.propertyId = propertyId;
    }

    public PropertyKey(string formatId, Int32 propertyId)
    {
        this.formatId = new Guid(formatId);
        this.propertyId = propertyId;
    }

}