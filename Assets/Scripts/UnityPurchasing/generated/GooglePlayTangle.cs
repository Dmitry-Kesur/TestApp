// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("K73+Vo2gkAO0QTzrPJRJ7Oqy6JV6E4aRIrWGjcZ5G8XvGaDOzF0EgJV6SHMoSFnWXacMICFFLq4EKJH8+0nK6fvGzcLhTYNNPMbKysrOy8iRtgY9OyX11QQX0WWXhht/5am/oLaNbYozIOKPifs/YVt7TLLKd/cQ74Apl8n7AhChmwfPL/MouFA4r51EzWUtxHrdUugA20oyfiWG/l0zf8caI8mlWnfnSL8wZtDGuPrjvY71fIcZV8xofeT6oF02VRinCssdawpyfxEm3PBRdjTEPaz4smKUaT5J6VD+V0G80x0jOV0skRr119XoDENfajTKq9/hU4hq6OMKZ3n6GWvz7dZJysTL+0nKwclJysrLVvLskG8XUbnfZLHt+aaTLMnIysvK");
        private static int[] order = new int[] { 4,12,6,4,4,10,7,7,11,9,11,13,13,13,14 };
        private static int key = 203;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
