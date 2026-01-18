namespace Bemo

open System
open System.Drawing

module SettingsValidator =

    type ValidationError = {
        Field: string
        Value: obj
        Message: string
    }

    type ValidationResult =
        | Valid
        | Invalid of ValidationError list

    /// Validates an integer is within a range
    let validateIntRange fieldName min max value =
        if value >= min && value <= max then
            Valid
        else
            Invalid [{
                Field = fieldName
                Value = box value
                Message = sprintf "%s must be between %d and %d (got %d)" fieldName min max value
            }]

    /// Validates a color is valid
    let validateColor fieldName (color: Color) =
        // Check if color is valid (not empty)
        if color.IsEmpty then
            Invalid [{
                Field = fieldName
                Value = box color
                Message = sprintf "%s cannot be an empty color" fieldName
            }]
        else
            Valid

    /// Validates tab appearance settings
    let validateTabAppearance (appearance: TabAppearance) =
        let errors = ResizeArray<ValidationError>()

        // Validate tabHeight (reasonable range: 15-50 pixels)
        match validateIntRange "tabHeight" 15 50 appearance.tabHeight with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        // Validate tabMaxWidth (reasonable range: 50-500 pixels)
        match validateIntRange "tabMaxWidth" 50 500 appearance.tabMaxWidth with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        // Validate tabOverlap (reasonable range: 0-50 pixels)
        match validateIntRange "tabOverlap" 0 50 appearance.tabOverlap with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        // Validate tabHeightOffset (reasonable range: 0-10 pixels)
        match validateIntRange "tabHeightOffset" 0 10 appearance.tabHeightOffset with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        // Validate tabIndentFlipped (reasonable range: 0-200 pixels)
        match validateIntRange "tabIndentFlipped" 0 200 appearance.tabIndentFlipped with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        // Validate tabIndentNormal (reasonable range: 0-50 pixels)
        match validateIntRange "tabIndentNormal" 0 50 appearance.tabIndentNormal with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        // Validate colors
        match validateColor "tabTextColor" appearance.tabTextColor with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        match validateColor "tabNormalBgColor" appearance.tabNormalBgColor with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        match validateColor "tabHighlightBgColor" appearance.tabHighlightBgColor with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        match validateColor "tabActiveBgColor" appearance.tabActiveBgColor with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        match validateColor "tabBorderColor" appearance.tabBorderColor with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        match validateColor "tabFlashBgColor" appearance.tabFlashBgColor with
        | Invalid errs -> errors.AddRange(errs)
        | Valid -> ()

        if errors.Count = 0 then
            Valid
        else
            Invalid (List.ofSeq errors)

    /// Sanitizes tab appearance by clamping values to valid ranges
    let sanitizeTabAppearance (appearance: TabAppearance) =
        let clamp min max value =
            if value < min then min
            elif value > max then max
            else value

        {
            tabHeight = clamp 15 50 appearance.tabHeight
            tabMaxWidth = clamp 50 500 appearance.tabMaxWidth
            tabOverlap = clamp 0 50 appearance.tabOverlap
            tabHeightOffset = clamp 0 10 appearance.tabHeightOffset
            tabIndentFlipped = clamp 0 200 appearance.tabIndentFlipped
            tabIndentNormal = clamp 0 50 appearance.tabIndentNormal
            tabTextColor = appearance.tabTextColor
            tabNormalBgColor = appearance.tabNormalBgColor
            tabHighlightBgColor = appearance.tabHighlightBgColor
            tabActiveBgColor = appearance.tabActiveBgColor
            tabBorderColor = appearance.tabBorderColor
            tabFlashBgColor = appearance.tabFlashBgColor
        }

    /// Gets a user-friendly error message from validation errors
    let formatValidationErrors (errors: ValidationError list) =
        let messages = errors |> List.map (fun e -> sprintf "• %s" e.Message)
        String.Join("\n", messages)

    /// Validates and sanitizes appearance, logging warnings
    let validateAndSanitize (appearance: TabAppearance) =
        match validateTabAppearance appearance with
        | Valid ->
            appearance
        | Invalid errors ->
            Logger.warning (sprintf "Invalid tab appearance settings detected:\n%s\nUsing sanitized values." (formatValidationErrors errors))
            sanitizeTabAppearance appearance
