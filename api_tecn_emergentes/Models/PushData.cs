namespace api_tecn_emergentes.Models
{
    public class PushData
    {
        public int id_entidad {get; set;}
        public bool riego {get; set;}
        public bool ventilacion{get; set;}

        public PushData(int i, bool r, bool v)
        {
            id_entidad = i;
            riego = r;
            ventilacion = v;
        }

        public PushData(){}
    }
}