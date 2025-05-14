namespace CactusLang.Model;

public interface IFieldHolder {
    public ModelField GetFieldGeneric(string name);

    public void AddFieldGeneric(ModelField field);

    public List<ModelField> GetFieldsGeneric();
}
