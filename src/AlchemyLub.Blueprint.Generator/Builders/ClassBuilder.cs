namespace AlchemyLub.Blueprint.Generator.Builders;

public sealed class ClassBuilder(string className)
{
    public List<FieldModel> Fields { get; }

    public List<ConstantModel> Constants { get; }

    public ConstructorModel? Constructor { get; private set; }

    public List<MethodModel> Methods { get; }

    public List<BaseClassModel> BaseClasses { get; }

    public ClassModel Build() => new(
        className,
        Fields,
        Constants,
        Methods,
        BaseClasses,
        Constructor);

    public ClassBuilder WithConstructor(ConstructorModel constructorModel)
    {
        Constructor = constructorModel;

        return this;
    }

    public ClassBuilder WithMethod(MethodModel methodModel)
    {
        Methods.Add(methodModel);

        return this;
    }

    public ClassBuilder WithMethods(IReadOnlyCollection<MethodModel> methodModels)
    {
        Methods.AddRange(methodModels);

        return this;
    }

    public ClassBuilder WithMethods(params MethodModel[] methodModels)
    {
        Methods.AddRange(methodModels);

        return this;
    }

    public ClassBuilder WithConstant(ConstantModel constantModel)
    {
        Constants.Add(constantModel);

        return this;
    }

    public ClassBuilder WithConstants(params ConstantModel[] constantModels)
    {
        Constants.AddRange(constantModels);

        return this;
    }

    public ClassBuilder WithConstants(IReadOnlyCollection<ConstantModel> constantModels)
    {
        Constants.AddRange(constantModels);

        return this;
    }

    public ClassBuilder WithField(FieldModel fieldModel)
    {
        Fields.Add(fieldModel);

        return this;
    }

    public ClassBuilder WithFields(params FieldModel[] fieldModels)
    {
        Fields.AddRange(fieldModels);

        return this;
    }

    public ClassBuilder WithFields(IReadOnlyCollection<FieldModel> fieldModels)
    {
        Fields.AddRange(fieldModels);

        return this;
    }

    public ClassBuilder WithBaseClass(BaseClassModel baseClass)
    {
        BaseClasses.Add(baseClass);

        return this;
    }

    public ClassBuilder WithBaseClasses(params BaseClassModel[] baseClasses)
    {
        BaseClasses.AddRange(baseClasses);

        return this;
    }

    public ClassBuilder WithBaseClasses(IReadOnlyCollection<BaseClassModel> baseClasses)
    {
        BaseClasses.AddRange(baseClasses);

        return this;
    }
}
