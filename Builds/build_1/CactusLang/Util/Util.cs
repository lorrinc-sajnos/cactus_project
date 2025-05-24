namespace CactusLang.Util;

public class UtilFunc {
    /// <summary>
    /// This is a foreach, where you can execute a function on the elements of collections, and you can execute a function between elements.
    /// </summary>
    public static void SeparatedForEach<T>(IList<T> collection, Action<T> actionOnElement, Action separatorAction) {
        for (int i = 0; i < collection.Count - 1; i++) {
            actionOnElement(collection[i]);
        }
        //Run last element
        actionOnElement(collection[^1]);
    }
}