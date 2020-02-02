module Feblr.Quant.OANDA

open Feblr.Quant.Engine.Currency
open Feblr.Quant.Engine.Account
open System.Net.Http
open FSharp.Control.Tasks.V2.ContextInsensitive
open System.Net.Http.Headers
open Thoth.Json.Net

type AccountType =
    | Practice
    | Trade

let restApiDomainPractice = "https://api-fxpractice.oanda.com/"
let restApiDomainTrade = "https://api-fxtrade.oanda.com"

let streamApiDomainPractice = "https://stream-fxpractice.oanda.com/"
let streamApiDomainTrade = "https://stream-fxtrade.oanda.com/"

// “-“-delimited string
// format “{siteID}-{divisionID}-{userID}-{accountNumber}”
type AccountID = string

type OANDAAccount =
    { id: AccountID
      alias: string
      currency: string
      // createdByUserID : (integer)
      // createdTime : (DateTime)
      // guaranteedStopLossOrderMode : (GuaranteedStopLossOrderMode)
      // resettablePLTime : (DateTime)
      marginRate: double
      openTradeCount: uint64
      // openPositionCount : (integer)
      // pendingOrderCount : (integer)
      // hedgingEnabled : (boolean)
      // lastOrderFillTimestamp : (DateTime)
      unrealizedPL: string
      NAV: string
      marginUsed: string
      marginAvailable: string
      // positionValue : string
      // marginCloseoutUnrealizedPL : string
      // marginCloseoutNAV : string
      // marginCloseoutMarginUsed : string
      // marginCloseoutPercent : (DecimalNumber)
      // marginCloseoutPositionValue : (DecimalNumber)
      // withdrawalLimit : (string
      // marginCallMarginUsed : string
      // marginCallPercent : (DecimalNumber)
      // pl: string
      balance: string }

type OANDAInstrument =
  { displayName: string
    displayPrecision: unit
    marginRate: string
    maximumOrderUnits: string
    maximumPositionSize: string
    maximumTrailingStopDistance: string
    minimumTradeSize: string
    minimumTrailingStopDistance: string
    name: string
    pipLocation: int
    tradeUnitsPrecision: int
    ``type``: string }

let client = new HttpClient()
client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue("application/json"))

let queryAccount (accountType: AccountType) (accountId: string) (accessToken: string) =
    task {
        let domain =
            match accountType with
            | Practice -> restApiDomainPractice
            | Trade -> restApiDomainTrade

        let uri = sprintf "%s/v3/accounts/%s" domain accountId
        let request = new HttpRequestMessage(HttpMethod.Get, uri)
        request.Headers.Authorization <- AuthenticationHeaderValue("Bearer", accessToken)
        let! response = client.SendAsync(request)
        let! body = response.Content.ReadAsStringAsync()
        let result =
            Decode.Auto.fromString<OANDAAccount> body
            |> Result.map (fun oandaAccount ->
              let currency = oandaAccount.currency
              let totalBalance = System.Double.Parse oandaAccount.balance
              let marginAvailable = System.Double.Parse oandaAccount.marginAvailable
              let marginRate = oandaAccount.marginRate
              Account<string>.create oandaAccount.id currency totalBalance marginAvailable marginRate
            )

        return result
    }

let queryAccountInstruments (accountType: AccountType) (accountId: string) (accessToken: string) =
    task {
        let domain =
            match accountType with
            | Practice -> restApiDomainPractice
            | Trade -> restApiDomainTrade

        let uri = sprintf "%s/v3/accounts/%s/instruments" domain accountId
        let request = new HttpRequestMessage(HttpMethod.Get, uri)
        request.Headers.Authorization <- AuthenticationHeaderValue("Bearer", accessToken)
        let! response = client.SendAsync(request)
        let! body = response.Content.ReadAsStringAsync()
        let result =
            Decode.Auto.fromString<{| instruments: OANDAInstrument seq |}> body

        return result
    }