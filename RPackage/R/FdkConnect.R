#' Initialize the CLR runtime and loads the FDK host assembly
#' 
ttInit <- function() {
  require(rClr)
  if(!require(stringi))
  {
    install.packages("stringi")
    require(stringi)
  }
  if(!require(data.table))
  {
    install.packages("data.table")
    require(data.table)
  }
  fileName <-system.file("data", "FdkRHost.dll", package="rFdk")
  clrLoadAssembly(fileName)
}
#' Connects to a TT server
#' 
#' @param address Url of the running server
#' @param login Account number you login
#' @param password Password for the account you login
#' @param fdkPath Path with connection related cache data
#' @param protocol Available protocols: FIX and LRP
#' @export
ttConnect <- function(address = "", login= "", password= "", fdkPath = "", protocol = "fix") {
  ttInit()
  rClr::clrCallStatic('RHost.FdkStatic', 'ConnectToFdk', address, login, password, fdkPath, protocol)
}

#' Disconnect from a TT server
#' 
#' @export
ttDisconnect <- function() {
  rClr::clrCallStatic('RHost.FdkStatic', 'Disconnect')
}

#' Is connected to TT server
#' 
#' @export
ttIsConnected <- function() {
  rClr::clrCallStatic('RHost.FdkStatic', 'IsConnected')
}

