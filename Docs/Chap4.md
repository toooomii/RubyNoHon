# 第４章：柔軟なインターフェースを作る
## インターフェースの認識
- 外部から使われることを想定したクラス内のメソッド
## パブリックインターフェース
外界から見たクラスのあり方
- クラスの主要な責任を明らかにする
- 外部から実行される
- 変更されにくい
- 依存しても安心
- テスト内で徹底的に文書化されている
## パブリックインターフェースを見つける
### 見当をつける
```Csharp
// example 4.3

class Customer
{
    private string plan;
    public MakeTripPlan()
    {
        this.Plan = Trip.SuitableTrips(on_date, of_difficulty, need_bike);
    }

    public Customer()
    {}
}

class MainClass
{
    public static void Main(string[] args)
    {
        Customer Moe = new Customer();
    }
}
```

```Csharp
// example 4.4

class Customer
{
    private string plan;
    public MakeTripPlan()
    {
        this.Plan = Trip.SuitableTrips(on_date, of_difficulty, need_bike);

        foreach (trip in this.Plan)
        {
            Bicycle.GoodFor(trip);
        }
    }

    public Customer()
    {}
}

class MainClass
{
    public static void Main(string[] args)
    {
        Customer Moe = new Customer();
    }
}
```

```Csharp
// example 4.5

class Customer
{
    private string plan;
    public MakeTripPlan()
    {
        this.Plan = Trip.SuitableTrips(on_date, of_difficulty, need_bike);
    }

    public Customer()
    {}
}

class MainClass
{
    public static void Main(string[] args)
    {
        Customer Moe = new Customer();
    }
}
```
```Csharp
// example 4.5

class Customer
{
    private string plan;
    public MakeTripPlan()
    {
        this.Plan = Trip.SuitableTrips(on_date, of_difficulty, need_bike);
    }

    public Customer()
    {}
}

class Mechanic
{

}

class Trip
{

}

class MainClass
{
    public static void Main(string[] args)
    {
        Customer Moe = new Customer();
    }
}
```
