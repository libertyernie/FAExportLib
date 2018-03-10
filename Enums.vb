' These enums need to be lowercase!

Public Enum FAFolder
    gallery
    scraps
    favorites
End Enum

Public Enum FAOrder
    relevancy
    [date]
    popularity
End Enum

Public Enum FAOrderDirection
    asc
    desc
End Enum

Public Enum FARange
    day
    week
    month
    all
End Enum

Public Enum FASearchMode
    all
    any
    extended
End Enum

<Flags>
Public Enum FARating
    general = 1
    mature = 2
    adult = 4
End Enum

<Flags>
Public Enum FAType
    art = 1
    flash = 2
    photo = 4
    music = 8
    story = 16
    poerty = 32
End Enum