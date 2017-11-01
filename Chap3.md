# ３章：依存関係の管理
## 前書き
1. オブジェクト間の相互作用は不可避
2. 如何にして他のオブジェクトで定義された振舞にアクセスするか？
3. 単一責任原則 -> 他のオブジェクトとの協力は必然

## 依存関係の理解
`int Rim`と`int Tire`がコンストラクタの引数に入っている -> 強い依存関係
``` Csharp
public class Gear
{
    private readonly int Cog;
    private readonly int Chainring;
    private readonly int Rim;
    private readonly int Tire;

    private double Ratio()
    {
        return (double)Cog / (double)Chainring;
    }

    private double GearInches()
    {
        // have to change here!
        // initializing the other class in class is not good.
        Wheel2 wheel = new Wheel2(this.Rim, this.Tire);
        return Ratio() * wheel.Diameter();
    }

    public Gear(int cog, int chainring, int rim, int tire)
    {
        this.Cog = cog;
        this.Chainring = chainring;
        this.Rim = rim;
        this.Tire = tire;
    }
}
```

## 依存関係の認識
オブジェクトは、以下のようなものを持っているときに別のオブジェクトに依存している
1. 他のクラスの名前：`Gear`は`Wheel`が存在することを期待している
2. メッセージの名前：`Gear`は`Wheel`の`Diameter`に反応することを期待している
3. メッセージの引数の名前：`Gear`は`Wheel`が`Rim`と`Tire`を必要とすることを知っている
4. 引数の順序：`Gear`は`Wheel`の引数が初め`Rim`で次が`Tire`と知っている
上記の場合にはいずれも`Wheel`が変更したときに`Gear`も変更されなければならない
### Coupling Between Objects
- 依存によってオブジェクトがカップリングされているという。
`Gear`が`Wheel`についてより多く知っていればいるほど、`Gear`は`Wheel`に強く依存している。（図参照）
- カップリングされたオブジェクトは一つのオブジェクトのように考える必要がある。
-> 一方を変更したらもう一方も変更する必要がある
- オブジェクトが強くカップリングされている場合には再利用は困難に
### その他の依存関係
1. あるオブジェクトが、他のオブジェクトが知っている別のオブジェクトについて知っている場合（連鎖的な依存関係）
->特に破壊的
->始点のオブジェクトが変更された場合にはすべてを修正する必要がある
2. テストによる依存関係
現在のコードに強くカップリングされたテストは変更に弱い
## ゆるくカップリングされたコードを書く戦略
### 依存性の注入
- `Gear`クラスの中でインスタンスを生成している
->`Rim`と`Tire`も`Gear`で管理しなければならない
->引数としてクラスを渡すことに
```Csharp
// Injecting dependencies.
public class Gear
{
    private readonly int Cog;
    private readonly int Chainring;
    private readonly Wheel Wheel;

    private double Ratio()
    {
        return (double)Cog / (double)Chainring;
    }

    public double GearInch()
    {
        return Ratio() * this.Wheel.Diameter();
    }

    public Gear(int cog, int chainring, Wheel wheel)
    {
        this.Cog = cog;
        this.Chainring = chainring;
        // put instantiating other class out of the class.
        // no rim and tire in this class anymore.
        this.Wheel = wheel;
    }
}
```
- このような手法を依存性の注入と呼ぶ
- `Gear`クラスが知っていることがより少なくなったのでより良いクラスに
### 依存の孤立化
- 依存関係を除去できないときには孤立化させる方針をとる
- 病気の隔離のようなもの
- 目的：クラス内に深く達している依存関係を明らかにすること
->コードの変更が比較的容易に
1. インスタンス生成の孤立化
例１：`Wheel`クラスのインスタンスの生成をコンストラクタに
```Csharp
// Isolate creation of instances.
public class Gear
{
    private readonly int Cog;
    private readonly int Chainring;
    private readonly Wheel Wheel;

    private double Ratio()
    {
        return (double)Cog / (double)Chainring;
    }

    public double GearInches()
    {
        return Ratio() * Wheel.Diameter();
    }

    public Gear(int cog, int chainring, int rim, int tire)
    {
        this.Cog = cog;
        this.Chainring = chainring;
        this.Wheel = new Wheel(rim, tire);
    }
}
```
例２：`Wheel`クラスのインスタンスの生成を行うメソッドを定義
```Csharp
// Isolate creation of instances.
public class Gear
{
    private readonly int Cog;
    private readonly int Chainring;
    private readonly int Rim;
    private readonly int Tire;
    private readonly Wheel Wheel;

    private double Ratio()
    {
        return (double)Cog / (double)Chainring;
    }

    public double GearInches()
    {
        // make instance explicitly.
        MakeWheel();
        return Ratio() * this.Wheel.Diameter();
    }

    public Gear(int cog, int chainring, int rim, int tire)
    {
        this.Cog = cog;
        this.Chainring = chainring;
        this.Rim = rim;
        this.Tire = tire;
    }

    private void MakeWheel()
    {
        this.Wheel = Wheel(this.Rim, this.Tire);
    }
}
```
2. 外部の脆弱なメッセージの孤立化
- 複雑な処理の中に外部からのメッセージが存在する場合には孤立化させる
```Csharp
public void GearInches()
{
    // complex process
    value = some_intermediate_result * Wheel.Diameter();
    // more complex process
}
```
```Csharp
public void GearInches()
{
    // complex process
    value = some_intermediate_result * Diameter();
    // more complex process
}

private double Diameter()
{
    return Wheel.Diameter();
}
```
### 引数順序依存性の除去
- map/hash/dictionary関数を使った順序依存性の除去
- 冗長な記述になる -> 利点になる
- 引数の順序への依存はなくなり、名称への依存ができる
 -> keyの名前がドキュメント代わりになる
```Csharp
// Use Hashes for initializing Argument.
public class Gear
{
    private readonly int Cog;
    private readonly int Chainring;
    private readonly Wheel Wheel;

    private double Ratio()
    {
        return (double)Cog / (double)Chainring;
    }

    public double GearInches()
    {
        // make instance explicitly.
        MakeWheel();
        return Ratio() * this.Wheel.Diameter();
    }

    public Gear(Dictionary<string, int> args)
    {
        this.Cog = args('Cog');
        this.Chainring = args('Chainring');
        this.Wheel = args('Wheel');
    }
}
```

### 複数パラメータ初期化の孤立化
```Csharp
namespace SomeFramework
{
    class Gear
    {
        private readonly int Cog;
        private readonly int Chainring;
        private readonly Wheel Wheel;

        public Gear(int cog, int chainring, Wheel wheel)
        {
            this.Cog = cog;
            this.Chainring = chainring;
            this.Wheel = wheel;
        }
    }
}

namespace GearWrapper
{
    SomeFramework::Gear gear(Dictionary<string, int> args)
    {
        SomeFramework::Gear gear = new SomeFramework::Gear(args['Cog'],
                          args['Chainring'],
                          args['Wheel']);
    }
}
```
## 依存方向の管理
### 依存方向の逆転
```Csharp
public class Gear
{
    private readonly int Cog;
    private readonly int Chainring;

    private double Ratio()
    {
        return (double)Cog / (double)Chainring;
    }

    private double GearInches()
    {
        Wheel2 wheel = new Wheel2(this.Rim, this.Tire);
        return Ratio() * wheel.Diameter();
    }

    public Gear(int cog, int chainring, int rim, int tire)
    {
        this.Cog = cog;
        this.Chainring = chainring;
        this.Rim = rim;
        this.Tire = tire;
    }
}
public class Wheel
{
    private readonly int Cog;
    private readonly int Chainring;
    private readonly Gear Gear;
}
```
## 依存方向の選択
格言「自分が変わるよりも少ない頻度で変わるものに依存せよ」
ー＞以下の理由による
- 要件が変更しやすいクラスは存在する
- コンクリートクラスは抽象クラスよりも変更されやすい
- 多くの依存を持つクラスの変更は広い影響を及ぼす

### 変更しやすさの理解
組み込みクラスと自分で作ったクラスはともに特異な変更しやすさが存在する

### コンクリートクラスと抽象クラスの認識
- 抽象クラスとは「どんなインスタンスとも関連しないもの」
- 依存性の注入
-

### 依存で満載したクラスの拒絶
- 依存されやすいクラスは避ける
### 問題のある依存性の発見
- 依存性の多い/少ない
- 変更しやすい/しにくい
で分類
ー＞
