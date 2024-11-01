namespace Zz.Services;

using Zz.App.Core;

public class Challenge : IChallenge
{
    public string RequestNewChallenge()
    {
        return string.Empty;
    }

    public string ValidateChallengeAnswer(string challengeAnswerData)
    {
        return string.Empty;
    }

    public bool ValidateChallengeTokenData(string challengeTokenData)
    {
        return true;
    }
}
