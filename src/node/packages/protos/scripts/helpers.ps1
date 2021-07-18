function Write {
    [CmdletBinding()]
    param (
        [Parameter(Position=0)]
        [String] $Text
    )

    $out = $False
    if ([bool]::TryParse($env:DEBUG, [ref]$out)) {
        # Inline doesn't work for some reason
        if ($out) {
            echo $Text
        }
    }
}
