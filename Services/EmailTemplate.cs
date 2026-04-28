namespace SportSistem.Services;

public static class EmailTemplate
{
    public static string BuildEmailHtml(string title, string intro, string bodyContent)
    {
        return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <style>
        body {{
            margin: 0;
            padding: 0;
            background: #080C10;
            color: #F0F2F5;
            font-family: 'Barlow', Arial, sans-serif;
        }}
        .email-shell {{
            max-width: 680px;
            margin: 0 auto;
            padding: 24px;
        }}
        .email-card {{
            background: #131920;
            border: 1px solid rgba(255,255,255,0.08);
            border-radius: 18px;
            overflow: hidden;
            box-shadow: 0 22px 60px rgba(0,0,0,0.22);
        }}
        .email-header {{
            padding: 28px 32px;
            background: linear-gradient(135deg, #0f1926 0%, #0d1117 100%);
            border-bottom: 1px solid rgba(255,255,255,0.08);
        }}
        .brand-title {{
            display: flex;
            align-items: center;
            gap: 12px;
            font-size: 1.4rem;
            font-weight: 800;
            text-transform: uppercase;
            letter-spacing: 0.08em;
            color: #F0F2F5;
        }}
        .brand-dot {{
            width: 12px;
            height: 12px;
            border-radius: 50%;
            background: #FF4500;
            box-shadow: 0 0 18px rgba(255,69,0,0.35);
        }}
        .email-body {{
            padding: 32px;
        }}
        .email-title {{
            margin: 0 0 16px;
            font-size: 1.5rem;
            color: #FFFFFF;
        }}
        .email-intro {{
            margin: 0 0 24px;
            font-size: 1rem;
            line-height: 1.7;
            color: rgba(240,242,245,0.9);
        }}
        .detail-block {{
            background: #0C131C;
            border: 1px solid rgba(255,255,255,0.08);
            border-radius: 14px;
            padding: 20px;
            margin-bottom: 20px;
        }}
        .detail-item {{
            margin-bottom: 16px;
        }}
        .detail-label {{
            display: block;
            margin-bottom: 6px;
            font-size: 0.8rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.06em;
            color: #FF4500;
        }}
        .detail-value {{
            font-size: 1rem;
            color: #F0F2F5;
            line-height: 1.6;
        }}
        .footer-text {{
            margin: 24px 0 0;
            font-size: 0.9rem;
            color: rgba(240,242,245,0.65);
            line-height: 1.6;
        }}
    </style>
</head>
<body>
    <div class=""email-shell"">
        <div class=""email-card"">
            <div class=""email-header"">
                <div class=""brand-title"">
                    <span class=""brand-dot""></span>
                    SportSystem Notification
                </div>
            </div>
            <div class=""email-body"">
                <h1 class=""email-title"">{title}</h1>
                <p class=""email-intro"">{intro}</p>
                <div class=""detail-block"">
                    {bodyContent}
                </div>
                <p class=""footer-text"">Gracias por usar SportSistem. Si necesitas ayuda, revisa tu panel de control o contacta con soporte.</p>
            </div>
        </div>
    </div>
</body>
</html>
";
    }

    public static string BuildDetailItem(string label, string value)
    {
        return $@"
            <div class=""detail-item"">
                <span class=""detail-label"">{label}</span>
                <span class=""detail-value"">{value}</span>
            </div>
        ";
    }
}