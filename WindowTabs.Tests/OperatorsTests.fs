module WindowTabs.Tests.OperatorsTests

open Xunit
open FsUnit.Xunit
open Bemo

[<Fact>]
let ``List2 should create empty list by default`` () =
    let list = List2()
    list.list |> should be Empty

[<Fact>]
let ``List2 should create list from items`` () =
    let items = [1; 2; 3]
    let list = List2(items)
    list.list |> should equal items

[<Fact>]
let ``List2 append should add items`` () =
    let list = List2([1; 2])
    let result = list.append 3
    result.list |> should equal [1; 2; 3]

[<Fact>]
let ``List2 prepend should add items at beginning`` () =
    let list = List2([2; 3])
    let result = list.prepend 1
    result.list |> should equal [1; 2; 3]

[<Fact>]
let ``List2 where should filter items`` () =
    let list = List2([1; 2; 3; 4; 5])
    let result = list.where (fun x -> x % 2 = 0)
    result.list |> should equal [2; 4]

[<Fact>]
let ``List2 map should transform items`` () =
    let list = List2([1; 2; 3])
    let result = list.map (fun x -> x * 2)
    result.list |> should equal [2; 4; 6]

[<Fact>]
let ``List2 distinct should remove duplicates`` () =
    let list = List2([1; 2; 2; 3; 3; 3])
    let result = list.distinct
    result.list |> should equal [1; 2; 3]

// Map2 Tests (after PowerPack removal)
[<Fact>]
let ``Map2 should create empty map by default`` () =
    let map = Map2<string, int>()
    map.items.list |> should be Empty

[<Fact>]
let ``Map2 should create map from list of tuples`` () =
    let items = List2([("a", 1); ("b", 2)])
    let map = Map2<string, int>(items)
    map.items.list.Length |> should equal 2
    map.contains "a" |> should be True
    map.contains "b" |> should be True

[<Fact>]
let ``Map2 add should add key-value pair`` () =
    let map = Map2<string, int>()
    let result = map.add "test" 42
    result.contains "test" |> should be True
    result.find "test" |> should equal 42

[<Fact>]
let ``Map2 remove should remove key`` () =
    let map = Map2<string, int>().add "test" 42
    let result = map.remove "test"
    result.contains "test" |> should be False

[<Fact>]
let ``Map2 tryFind should return Some when key exists`` () =
    let map = Map2<string, int>().add "test" 42
    match map.tryFind "test" with
    | Some value -> value |> should equal 42
    | None -> failwith "Expected Some value"

[<Fact>]
let ``Map2 tryFind should return None when key does not exist`` () =
    let map = Map2<string, int>()
    map.tryFind "nonexistent" |> should equal None

[<Fact>]
let ``Map2 keys should return all keys`` () =
    let map = Map2<string, int>().add "a" 1 |> fun m -> m.add "b" 2
    let keys = map.keys.list |> List.sort
    keys |> should equal ["a"; "b"]

[<Fact>]
let ``Map2 values should return all values`` () =
    let map = Map2<string, int>().add "a" 1 |> fun m -> m.add "b" 2
    let values = map.values.list |> List.sort
    values |> should equal [1; 2]

// Set2 Tests (after PowerPack removal)
[<Fact>]
let ``Set2 should create empty set by default`` () =
    let set = Set2<int>()
    set.items.list |> should be Empty

[<Fact>]
let ``Set2 should create set from list`` () =
    let items = List2([1; 2; 3])
    let set = Set2<int>(items)
    set.items.list.Length |> should equal 3

[<Fact>]
let ``Set2 should automatically remove duplicates`` () =
    let items = List2([1; 2; 2; 3; 3; 3])
    let set = Set2<int>(items)
    set.items.list |> List.sort |> should equal [1; 2; 3]

[<Fact>]
let ``Set2 add should add item`` () =
    let set = Set2<int>()
    let result = set.add 42
    result.items.list |> should contain 42

[<Fact>]
let ``Set2 remove should remove item`` () =
    let set = Set2<int>().add 42
    let result = set.remove 42
    result.items.list |> should be Empty

[<Fact>]
let ``Set2 contains should return true for existing items`` () =
    let set = Set2<int>().add 42
    set.contains 42 |> should be True

[<Fact>]
let ``Set2 contains should return false for non-existing items`` () =
    let set = Set2<int>()
    set.contains 42 |> should be False

[<Fact>]
let ``Set2 union should combine sets`` () =
    let set1 = Set2<int>().add 1 |> fun s -> s.add 2
    let set2 = Set2<int>().add 2 |> fun s -> s.add 3
    let result = set1.union set2
    result.items.list |> List.sort |> should equal [1; 2; 3]

[<Fact>]
let ``Set2 intersect should return common elements`` () =
    let set1 = Set2<int>().add 1 |> fun s -> s.add 2 |> fun s -> s.add 3
    let set2 = Set2<int>().add 2 |> fun s -> s.add 3 |> fun s -> s.add 4
    let result = set1.intersect set2
    result.items.list |> List.sort |> should equal [2; 3]

[<Fact>]
let ``Set2 difference should return elements in first but not second`` () =
    let set1 = Set2<int>().add 1 |> fun s -> s.add 2 |> fun s -> s.add 3
    let set2 = Set2<int>().add 2 |> fun s -> s.add 3
    let result = set1.difference set2
    result.items.list |> should equal [1]
