// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Online;
using osu.Game.Scoring;
using osu.Game.Screens.Ranking.Statistics.User;
using osu.Game.Users;

namespace osu.Game.Tests.Visual.Ranking
{
    public partial class TestSceneOverallRanking : OsuTestScene
    {
        private readonly Bindable<ScoreBasedUserStatisticsUpdate?> statisticsUpdate = new Bindable<ScoreBasedUserStatisticsUpdate?>();

        [Test]
        public void TestUpdatePending()
        {
            createDisplay();
        }

        [Test]
        public void TestAllIncreased()
        {
            createDisplay();
            displayUpdate(
                new UserStatistics
                {
                    GlobalRank = 12_345,
                    Accuracy = 98.99,
                    MaxCombo = 2_322,
                    RankedScore = 23_123_543_456,
                    TotalScore = 123_123_543_456,
                    PP = 5_072
                },
                new UserStatistics
                {
                    GlobalRank = 1_234,
                    Accuracy = 99.07,
                    MaxCombo = 2_352,
                    RankedScore = 23_124_231_435,
                    TotalScore = 123_124_231_435,
                    PP = 5_434
                });
        }

        // cross-reference: `TestSceneToolbarUserButton.TestTransientUserStatisticsDisplay()`, "Test rounding treatment" step.
        [Test]
        public void TestRoundingTreatment()
        {
            createDisplay();
            displayUpdate(
                new UserStatistics
                {
                    GlobalRank = 12_345,
                    Accuracy = 98.99,
                    MaxCombo = 2_322,
                    RankedScore = 23_123_543_456,
                    TotalScore = 123_123_543_456,
                    PP = 5_071.495M
                },
                new UserStatistics
                {
                    GlobalRank = 12_345,
                    Accuracy = 98.99,
                    MaxCombo = 2_322,
                    RankedScore = 23_123_543_456,
                    TotalScore = 123_123_543_456,
                    PP = 5_072.99M
                });
        }

        [Test]
        public void TestAllDecreased()
        {
            createDisplay();
            displayUpdate(
                new UserStatistics
                {
                    GlobalRank = 1_234,
                    Accuracy = 99.07,
                    MaxCombo = 2_352,
                    RankedScore = 23_124_231_435,
                    TotalScore = 123_124_231_435,
                    PP = 5_434
                },
                new UserStatistics
                {
                    GlobalRank = 12_345,
                    Accuracy = 98.99,
                    MaxCombo = 2_322,
                    RankedScore = 23_123_543_456,
                    TotalScore = 123_123_543_456,
                    PP = 5_072
                });
        }

        [Test]
        public void TestNoChanges()
        {
            var statistics = new UserStatistics
            {
                GlobalRank = 12_345,
                Accuracy = 98.99,
                MaxCombo = 2_322,
                RankedScore = 23_123_543_456,
                TotalScore = 123_123_543_456,
                PP = 5_072
            };

            createDisplay();
            displayUpdate(statistics, statistics);
        }

        [Test]
        public void TestNotRanked()
        {
            var statistics = new UserStatistics
            {
                GlobalRank = null,
                Accuracy = 98.99,
                MaxCombo = 2_322,
                RankedScore = 23_123_543_456,
                TotalScore = 123_123_543_456,
                PP = null
            };

            createDisplay();
            displayUpdate(statistics, statistics);
        }

        [Test]
        public void TestFromNothing()
        {
            createDisplay();
            displayUpdate(
                new UserStatistics(),
                new UserStatistics
                {
                    GlobalRank = 12_345,
                    Accuracy = 98.99,
                    MaxCombo = 2_322,
                    RankedScore = 23_123_543_456,
                    TotalScore = 123_123_543_456,
                    PP = 5_072
                });
        }

        [Test]
        public void TestToNothing()
        {
            createDisplay();
            displayUpdate(
                new UserStatistics
                {
                    GlobalRank = 12_345,
                    Accuracy = 98.99,
                    MaxCombo = 2_322,
                    RankedScore = 23_123_543_456,
                    TotalScore = 123_123_543_456,
                    PP = 5_072
                },
                new UserStatistics());
        }

        private void createDisplay() => AddStep("create display", () =>
        {
            statisticsUpdate.Value = null;
            Child = new OverallRanking(new ScoreInfo())
            {
                Width = 400,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                DisplayedUpdate = { BindTarget = statisticsUpdate }
            };
        });

        private void displayUpdate(UserStatistics before, UserStatistics after) =>
            AddStep("display update", () => statisticsUpdate.Value = new ScoreBasedUserStatisticsUpdate(new ScoreInfo(), before, after));
    }
}
