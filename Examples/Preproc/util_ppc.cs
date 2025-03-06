/*

*/

const int[] large_primes = {2, 3, 5, 7, 11};

[CactusPreprocComponent]
class UtilPpc{

    [Func]
    I32 RandPrime(){
        return I32.FromInt(large_primes.GetRandom());
    }

    //Can override name
    [Func("const_rand_prime")]
    I32 ConstantPrime(I32 seed){
        throw new NotImplementedException();
    }

    [Generator]
    void GenObfCode(CodeContext context){
        string code = """
        i32 a = 13;
        i32 b = a*b*123213+1321-3;
        i32 c;

        b = a - c + 1231 * 55567;
        """
        context.Row().InsertBelow(code);
    }
	
    [Tag]
    void Public(CodeContext context, Function func){
        context.Header().InsertTop(func.Header());
    }
}


