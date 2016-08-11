

#' Gets the bars pairs as requested
#' 
#' @param symbol Symbol looked
#' @param depth Depths of quotes info
ttGetQuotesInfo <- function(symbol, depth) {
  rClr::clrCallStatic('RHost.FdkBars', 'ComputeGetQuotesInfo', symbol, depth)
}

#' Gets the bars pairs as requested
#' 
#' @param symbol Symbol looked
#' @param priceTypeStr Ask or bid
#' @param barPeriodStr Values like: M1, H1
ttGetBarsInfo <- function(symbol, priceTypeStr, barPeriodStr) {
  rClr::clrCallStatic('RHost.FdkBars', 'ComputeGetBarsInfo', symbol, priceTypeStr, barPeriodStr)
}

#' Get the list of referenced variables
#' 
ttVarList <- function() {
  rClr::clrCallStatic('RHost.FdkVars', 'GetVarNames')
}

#' unregister a variable
#' 
#' @param varName .Net variable to be removed
UnregisterVar <- function(varName) {
  rClr::clrCallStatic('RHost.FdkVars', 'Unregister', varName)
}

#' Clear the environment variables
#' 
#' @param varName .Net variable to be removed
ttUnregisterAllVariables <- function() {
  rClr::clrCallStatic('RHost.FdkVars', 'ClearAll')
}

