# 第５章 ダックタイピングでコスト削減
## ダックを見逃す
ダックタイプ
- 重要なのはパブリックインターフェース：クラスはオブジェクトがインターフェースを獲得するための方法の１つ
- クラスにとって重要なのは「何であるか」ではなく「何をするか」
- 「ダックのように鳴き、ダックのように歩くものがあればそれはダックだ」
ダックタイプを使わないコードからダックタイプを見つけることで
```Csharp
class Trip
{
    public void prepare(IPreparers mechanic)
    {
        mechanic.PrepareBicycles(Bicycles bicycles);
    }
}

class Mechanic : IPreparers
{
    public void PrepareBicycles(bicycles)
    {
        foreach (Bicycle bicycle in bicycles)
        {
            PrepareBicycle(bicycle);
        }
    }

    public void PrepareBicycle(bicycle)
    {
        return;
    }
}
```
このクラスは`Mechanic`クラスに明示的に依存しているわけではないが、メソッド`PrepareBicycles(Bicycles bicycles)`を持つクラスに依存している。
### 問題を悪化させる
要件が複雑化
- コーディネーターと運転手が必要になった
-> Tripのprepareメソッドを変更
```Csharp
class Trip
{
    public void Prepare(IPreparers preparers)
    {
        foreach(Preparer preparer in preparers)
        {
            switch(preparer.GetType().FullName)
            {
                case "Mechanic":
                    preparer.PrepareBicycles(bicycles);
                case "TripCoordinator":
                    preparer.BuyFood(customers);
                case "Driver":
                    preparer.GasUp(vehicle);
                    preparer.FillWaterTank(vehicle);
            }
        }
    }
}

class Mechanic : IPreparers
{
    public void PrepareBicycles(bicycles)
    {
        foreach (Bicycle bicycle in bicycles)
        {
            PrepareBicycle(bicycle);
        }
    }

    public void PrepareBicycle(bicycle)
    {
        return;
    }
}

class TripCoordinator : IPreparers
{
    public void BuyFood(customers)
    {
        return;
    }
}

class Driver : IOccupation
{
    public void GasUp(vehicle)
    {
        return;
    }
    public void FillWaterTank(vehicle)
    {
        return;
    }
}
```
- 状況を悪化させている(依存満載のクラス)
- `prepare`メソッドがメカニックを要求しているという想定に引きずられている
- インスタンスのインターフェースが`Driver`や`TripCoordinator`のインスタンスを自然と見つけにいく動作が`prepare`の望む動作
### ダックを見つけに行く
- インターフェース`Preparer`を用意して`prepare`メソッドを呼び出す作戦
->個別のクラスの定義は要らない

```Csharp
class Trip
{
    private Bicycles Bicycles { get; }
    private Customers Customers { get; }

    public void Prepare(IPreparers prepares)
    {
        foreach (IPreparers preparer in preparers)
        {
            preparer.PrepareTrip();
        }
    }
}
abstract class IPreparers
{
    abstract public void PrepareTrip(trip)
    {}
}

class Mechanic : IPreparers
{
    public void PrepareTrip(trip)
    {
        foreach (Bicycle bicycle in trip.bicycles)
        {
            PrepareBicycle(bicycle);
        }
    }
    // ...
}

class TripCoordinator : IPreparers
{
    public void PrepareTrip(trip)
    {
        BuyFood(trip.customers);
    }
}

class Driver : IOccupation
{
    public void PrepareTrip(trip)
    {
        vehicle = trip.vehicle
        GasUp(vehicle);
        FillWaterTank(vehicle);
    }
}
```
### ダックタイピングの影響
- 具象から抽象へ
- 読解が難しくなったかわりに拡張が容易に
## ダックを信頼するコードを書く
### 隠れたダックを認識する
以下の３つはダックで置き換えられる
- クラスで分岐するcase文
- kind_ofとis_a
- responds_to
#### クラスで分岐するcase文
先程の`Trip`クラス
#### `kind_of`と`is_a`
```Csharp
if (preparer.kind_of("Mechanic"))
{
    preparer.PrepareBicycles(bicycle);
}
else if (preparer.kind_of("TripCoordinator"))
{
    preparer.BuyFood(customers);
}
else if (preparer.kind_of("Driver"))
{
    preparer.GasUp(vehicle);
    preparer.FillWaterTank(vehicle);
}
```
- 結局case文を使っているのでNG
- クラスの明示的な参照は使っていないが、明らかに想定している
### ダックを信頼する
- ここまでのコードの問題点はメッセージの出元のオブジェクトの素性を知っていながらメッセージを利用していること
- インターフェースが正しく振る舞ってくれることを信頼して、実装する
### ダックタイプを文書化する
- `preparer`は抽象的な存在なので文書化が必要
- ->テスト（＝ドキュメンテーション）を書く必要がある
### ダック間でコードを共有
- `Mechanic`、`Driver`、`TripCoordinator`がそれぞれ`PrepareTrip`を実装している
- ただし、振舞いとしても共有する必要があることがある
### 賢くダックを選ぶ
- `Ruby on Rails`の例
明らかにif elseで条件分岐している
->なぜ受け入れられるのか？
- クラスの安定性
依存しているのはRubyのコアクラスのみ
->安定したクラスへは依存してもよい
## ダックタイピングへの恐れを克服する
動的型付と静的型付の比較
### 静的型付によるダックタイピングの無効化
静的型付の支持者は型エラーを取り除くために型検査を導入する傾向にある
->ダックタイピングにより型検査をすべて取り除くことができる
### 静的型付と動的型付
静的型付の利点
- コンパイラがコンパイル時に型エラーを発見してくれる
- 可視化された型情報により文書化の役割もある
- コンパイルにより最適化され高速に動作する
以下の仮定が成立するときのみ上記は利点になる
- コンパイラによる型検査を経ないと実行時にエラーが発生する
- 型がなければプログラマがコードを理解できない
- 最適化がなければコードが遅すぎる
動的型付の利点
- コードは逐次実行されコンパイル/makeサイクルがない
- ソースコードが型情報を含まない
- メタプログラミングが簡単
利点の仮定
- アプリケーションの開発がコンパイル/makeサイクルがない方が高速
- コンテキストからオブジェクトの型を推測できる
- メタプログラミングがあった方が望ましい言語機能
### 動的型付を受け入れる
- 実行速度をどうしても上げたければ静的型付が不可欠
- メタプログラミングは（適切に書かれれば）大きな価値がある
->動的型付に利点
- コンパイル時の型エラー
-- コンパイラは不慮の型エラーから救ってくれる
-- コンパイラなしでは型エラーから逃れられない
->キャスト可能な言語では結局同じ（実行時エラーが起こる）
->実行時型エラーは結局起こる
- 動的型付によりコンパイル時の型検査を取り除く莫大な効率性を提供してくれる
