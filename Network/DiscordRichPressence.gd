extends Node


func _ready():
    print("Discord SDK Wrapper ready")
    discord_sdk.app_id = 1154891864487493773

func _set(property, value):
    if(property == "app_id"):
        discord_sdk.app_id = value
        return true
    elif(property == "details"):
        discord_sdk.details = value
        return true
    elif(property == "state"):
        discord_sdk.state = value
        return true
    elif(property == "large_image"):
        discord_sdk.large_image = value
        return true
    elif(property == "large_image_text"):
        discord_sdk.large_image_text = value
        return true
    elif(property == "small_image"):
        discord_sdk.small_image = value
        return true
    elif(property == "small_image_text"):
        discord_sdk.small_image_text = value
        return true
    elif(property == "start_timestamp"):
        discord_sdk.start_timestamp = value
        return true
    elif(property == "end_timestamp"):
        discord_sdk.end_timestamp = value
        return true
    elif(property == "current_party_size"):
        discord_sdk.current_party_size = value
        return true
    elif(property == "max_party_size"):
        discord_sdk.max_party_size = value
        return true
    elif(property == "join_secret"):
        discord_sdk.join_secret = value
        return true
    elif(property == "match_secret"):
        discord_sdk.match_secret = value
        return true
    elif(property == "spectate_secret"):
        discord_sdk.spectate_secret = value
        return true
    elif(property == "party_id"):
        discord_sdk.party_id = value
        return true
    elif(property == "is_party_public"):
        discord_sdk.is_public_party = value
        return true
    elif(property == "is_overlay_locked"):
        discord_sdk.is_overlay_locked = value
        return true
    elif(property == "instanced"):
        discord_sdk.instanced = value;
    return false
    
func _get(property):
    if(property == "app_id"):
        return discord_sdk.app_id
    elif(property == "details"):
        return discord_sdk.details
    elif(property == "state"):
        return discord_sdk.state
    elif(property == "large_image"):
        return discord_sdk.large_image
    elif(property == "large_image_text"):
        return discord_sdk.large_image_text
    elif(property == "small_image"):
        return discord_sdk.small_image
    elif(property == "small_image_text"):
        return discord_sdk.small_image_text
    elif(property == "start_timestamp"):
        return discord_sdk.start_timestamp
    elif(property == "end_timestamp"):
        return discord_sdk.end_timestamp
    elif(property == "current_party_size"):
        return discord_sdk.current_party_size
    elif(property == "max_party_size"):
        return discord_sdk.max_party_size
    elif(property == "join_secret"):
        return discord_sdk.join_secret
    elif(property == "match_secret"):
        return discord_sdk.match_secret
    elif(property == "spectate_secret"):
        return discord_sdk.spectate_secret
    elif(property == "party_id"):
        return discord_sdk.party_id
    elif(property == "is_party_public"):
        return discord_sdk.is_public_party
    elif(property == "is_overlay_locked"):
        return discord_sdk.is_overlay_locked
    elif(property == "instanced"):
        return discord_sdk.instanced
    return null

static func refresh():
    discord_sdk.refresh()


static func accept_invite(user_id: int):
    discord_sdk.accept_invite(user_id)

static func clear(reset_values: bool):
    discord_sdk.clear(reset_values)

static func unclear():
    discord_sdk.unclear()

static func register_command(command: String):
    discord_sdk.register_command(command)

static func accept_join_request(user_id: int):
    discord_sdk.accept_join_request(user_id)

static func coreupdate():
    discord_sdk.coreupdate()

static func debug():
    discord_sdk.debug()

static func register_steam(steam_id: int):
    discord_sdk.register_steam(steam_id)

static func get_all_relationships():
    discord_sdk.get_all_relationships()

static func get_current_user():
    discord_sdk.get_current_user()

static func get_is_overlay_enabled():
    discord_sdk.get_is_overlay_enabled()

static func open_invite_overlay(is_spectate: bool):
    discord_sdk.open_invite_overlay(is_spectate)

static func open_server_invite_overlay(invite_code: String):
    discord_sdk.open_server_invite_overlay(invite_code)

static func open_voice_settings():
    discord_sdk.open_voice_settings()

static func get_is_discord_working():
    return discord_sdk.get_is_discord_working()

static func get_result_int():
    return discord_sdk.get_result_int()



