extends Node

func debug(message:String,values={}):
    GodotLogger.debug(message, values)

func info(message:String, values={}):
    GodotLogger.info(message, values)

func warn(message:String,values={}):
    GodotLogger.warn(message, values)

func error(message:String,values={}):
    GodotLogger.error(message, values)

func fatal(message:String,values={}):
    GodotLogger.fatal(message, values)