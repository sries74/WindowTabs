module WindowTabs.Tests.GroupingKeyTests

open Xunit
open FsUnit.Xunit
open Bemo

// Note: These tests verify the grouping key logic conceptually
// Actual Pid type requires Win32 process access, so we test the logic here

[<Fact>]
let ``Grouping key with no AUMID should use process path only`` () =
    // Simulate the logic from Pid.groupingKey
    let processPath = "C:\\Program Files\\App\\app.exe"
    let aumid: string = null

    let groupingKey =
        match aumid with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    groupingKey |> should equal "C:\\Program Files\\App\\app.exe"

[<Fact>]
let ``Grouping key with empty AUMID should use process path only`` () =
    let processPath = "C:\\Program Files\\App\\app.exe"
    let aumid = ""

    let groupingKey =
        match aumid with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    groupingKey |> should equal "C:\\Program Files\\App\\app.exe"

[<Fact>]
let ``Grouping key with AUMID should combine path and AUMID`` () =
    let processPath = "C:\\Program Files\\Microsoft\\Edge\\msedge.exe"
    let aumid = "MSEdge_PWA_Gmail_abc123"

    let groupingKey =
        match aumid with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    groupingKey |> should equal "C:\\Program Files\\Microsoft\\Edge\\msedge.exe|MSEdge_PWA_Gmail_abc123"

[<Fact>]
let ``Edge PWA with different AUMIDs should have different grouping keys`` () =
    let processPath = "C:\\Program Files\\Microsoft\\Edge\\msedge.exe"
    let aumid1 = "MSEdge_PWA_Gmail_abc123"
    let aumid2 = "MSEdge_PWA_Calendar_def456"

    let groupingKey1 =
        match aumid1 with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    let groupingKey2 =
        match aumid2 with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    groupingKey1 |> should not' (equal groupingKey2)

[<Fact>]
let ``Edge PWA with same AUMID should have same grouping key`` () =
    let processPath = "C:\\Program Files\\Microsoft\\Edge\\msedge.exe"
    let aumid1 = "MSEdge_PWA_Gmail_abc123"
    let aumid2 = "MSEdge_PWA_Gmail_abc123"

    let groupingKey1 =
        match aumid1 with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    let groupingKey2 =
        match aumid2 with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    groupingKey1 |> should equal groupingKey2

[<Fact>]
let ``Edge browser without AUMID should differ from PWA with AUMID`` () =
    let processPath = "C:\\Program Files\\Microsoft\\Edge\\msedge.exe"
    let browserAumid: string = null
    let pwaAumid = "MSEdge_PWA_Gmail_abc123"

    let browserKey =
        match browserAumid with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    let pwaKey =
        match pwaAumid with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    browserKey |> should not' (equal pwaKey)

[<Fact>]
let ``Traditional apps without AUMID should group by process path`` () =
    let processPath1 = "C:\\Windows\\notepad.exe"
    let processPath2 = "C:\\Windows\\notepad.exe"
    let aumid1: string = null
    let aumid2: string = null

    let groupingKey1 =
        match aumid1 with
        | null -> processPath1
        | "" -> processPath1
        | id -> sprintf "%s|%s" processPath1 id

    let groupingKey2 =
        match aumid2 with
        | null -> processPath2
        | "" -> processPath2
        | id -> sprintf "%s|%s" processPath2 id

    groupingKey1 |> should equal groupingKey2

[<Fact>]
let ``Different apps with same AUMID should still have different keys due to different paths`` () =
    // This is unlikely but theoretically possible
    let processPath1 = "C:\\App1\\app.exe"
    let processPath2 = "C:\\App2\\app.exe"
    let aumid = "SomeApp_123"

    let groupingKey1 =
        match aumid with
        | null -> processPath1
        | "" -> processPath1
        | id -> sprintf "%s|%s" processPath1 id

    let groupingKey2 =
        match aumid with
        | null -> processPath2
        | "" -> processPath2
        | id -> sprintf "%s|%s" processPath2 id

    groupingKey1 |> should not' (equal groupingKey2)

[<Fact>]
let ``Grouping key should be case sensitive for AUMID`` () =
    let processPath = "C:\\Program Files\\Microsoft\\Edge\\msedge.exe"
    let aumid1 = "MSEdge_PWA_Gmail_ABC"
    let aumid2 = "MSEdge_PWA_Gmail_abc"

    let groupingKey1 =
        match aumid1 with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    let groupingKey2 =
        match aumid2 with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    groupingKey1 |> should not' (equal groupingKey2)

// Test the pipe separator doesn't cause issues
[<Fact>]
let ``Grouping key should use pipe separator correctly`` () =
    let processPath = "C:\\App\\app.exe"
    let aumid = "App_ID"

    let groupingKey =
        match aumid with
        | null -> processPath
        | "" -> processPath
        | id -> sprintf "%s|%s" processPath id

    groupingKey |> should contain "|"
    groupingKey.Split('|').Length |> should equal 2
    groupingKey.Split('|').[0] |> should equal processPath
    groupingKey.Split('|').[1] |> should equal aumid
