// Simple helper to return the "exMaxLen" attribute for the specified
// field.  Using "getAttribute" won't work with Firefox.
function GetMaxLength(targetField)
{
    return targetField.exMaxLen;
}

// Limit the text input in the specified field.
function LimitInput(targetField, sourceEvent)
{
    var isPermittedKeystroke,
        enteredKeystroke,
        maximumFieldLength,
        currentFieldLength,
        inputAllowed = true,
        selectionLength = parseInt(GetSelectionLength(targetField));

    if (GetMaxLength(targetField) == null)
    {
        sourceEvent.returnValue = inputAllowed;
        return (inputAllowed);
    }

    // Get the current and maximum field length
    currentFieldLength = parseInt(targetField.value.length);
    maximumFieldLength = parseInt(GetMaxLength(targetField));

    // Allow non-printing, arrow and delete keys
    enteredKeystroke = window.event ? sourceEvent.keyCode : sourceEvent.which;
    isPermittedKeystroke = ((enteredKeystroke < 32) || (enteredKeystroke >= 33 && enteredKeystroke <= 40) || (enteredKeystroke === 46));

    // Decide whether the keystroke is allowed to proceed
    if (!isPermittedKeystroke)
    {
        if ((currentFieldLength - selectionLength) >= maximumFieldLength)
        {
            inputAllowed = false;
        }
    }

    // Force a trim of the textarea contents if necessary
    if (currentFieldLength > maximumFieldLength)
    {
        targetField.value = targetField.value.substring(0, maximumFieldLength);
    }

    sourceEvent.returnValue = inputAllowed;
    return (inputAllowed);
}

// Limit the text input in the specified field.
function LimitPaste(targetField, sourceEvent)
{
    var clipboardText,
        resultantLength,
        maximumFieldLength,
        currentFieldLength,
        pasteAllowed = true,
        selectionLength = GetSelectionLength(targetField);

    if (GetMaxLength(targetField) == null)
    {
        sourceEvent.returnValue = pasteAllowed;
        return (pasteAllowed);
    }

    // Get the current and maximum field length
    currentFieldLength = parseInt(targetField.value.length);
    maximumFieldLength = parseInt(GetMaxLength(targetField));

    clipboardText = window.clipboardData.getData("Text");
    resultantLength = currentFieldLength + clipboardText.length - selectionLength;

    if (resultantLength > maximumFieldLength)
    {
        pasteAllowed = false;
    }

    sourceEvent.returnValue = pasteAllowed;
    return (pasteAllowed);
}

// Returns the number of selected characters in the specified element
function GetSelectionLength(targetField)
{
    if (targetField.selectionStart === undefined)
    {
        return document.selection.createRange().text.length;
    }

    return (targetField.selectionEnd - targetField.selectionStart);
}