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
    public class ExtObject<K, V> : IExtObject<K, V>
    {
        private K asKey;
        private V asValue;

        public ExtObject()
        {
            this.asKey = default(K);
            this.asValue = default(V);
        }

        public ExtObject(K key, V value)
        {
            this.asKey = key;
            this.asValue = value;
        }

        public K Key
        {
            get { return this.asKey; }
        }

        public V Value
        {
            get { return this.asValue; }
        }

        public override string ToString()
        {
            return "K is " + typeof(K).ToString() + ". V is " + typeof(V).ToString();
        }
    }
}