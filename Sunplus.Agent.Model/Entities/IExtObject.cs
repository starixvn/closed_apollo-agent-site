/*
 * Created 20111227@Andy - Define Key-Value pair object
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.Entities
{
    /// <summary>
    /// Define Key-Value pair object
    /// </summary>
    public interface IExtObject<K, V>
    {
        K Key { get; }

        V Value { get; }
    }
}