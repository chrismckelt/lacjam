
namespace Lacjam.Framework.Model
{

    public interface IIdentity<out TKey>
    {
        TKey GetIdentity();
    }

}
